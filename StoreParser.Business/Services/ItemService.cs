using StoreParser.Data;
using System;
using System.Linq;
using StoreParser.Dtos;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Threading.Tasks;

namespace StoreParser.Business
{
    public class ItemService : IParseService
    { 
        Parser _parser;
        protected readonly IUnitOfWork _uow;
        protected readonly IMapper _mapper;

        public ItemService(IUnitOfWork uow, Parser parser, IMapper mapper)
        {
            _uow = uow;
            _parser = parser;
            _mapper = mapper;
        }

        public IEnumerable<NewItemDto> AddItems(ParsingConfiguration config)
        {

            var items = GetParsedItem(config);
            var newRecords = _mapper.Map<IEnumerable<NewItemDto>>(items);
            SaveParsedItems(items);

            return newRecords;
        }

        public IEnumerable<ItemDto> GetAll()
        {
            IList<ItemDto> list = new List<ItemDto>();
            var items = _uow.Items.GetAll();     

            foreach (var item in items)
            {
                list.Add(ToItemDto(item));
            }

            return list;
        }

        public ItemDto GetById(int id)
        {
            var item = _uow.Items.Get(id);

            return ToItemDto(item);
        }

        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {
            IList<ItemDto> list = new List<ItemDto>();
            var items = await _uow.Items.GetAllAsync();

            foreach (var item in items)
            {
                list.Add(ToItemDto(item));
            }
            return list;
        }

        public async Task<IEnumerable<NewItemDto>> AddItemsAsync(ParsingConfiguration config)
        {
            var items = GetParsedItem(config);
            var newRecords = _mapper.Map<IEnumerable<NewItemDto>>(items);
            await SaveParsedItemsAsync(items);
            return newRecords;
        }

        public async Task<ItemDto> GetByIdAsync(int id)
        {
            var item = await _uow.Items.GetAsync(id);
            return ToItemDto(item);
        }
                
        protected virtual ItemDto ToItemDto(Item item)
        {
            ItemDto itemDto = _mapper.Map<ItemDto>(item);
            itemDto.Prices = item.Prices.Select(pr => new PriceDto { Date = pr.Date, Price = pr.CurrentPrice });

            return itemDto;
        }

        protected virtual Price ToPrice(ParseResultItem itemDto, Item item)
        {
            var price = new Price
            {
                Item = item,
                CurrentPrice = itemDto.Price,
                Date = DateTime.Now
            };
            return price;
        }

        IEnumerable<ParseResultItem> GetParsedItem(ParsingConfiguration config)
        {
            IParserStategy strategy;//Use strategy pattern

            switch (config.Stategy)
            {
                case ParseStategyEnum.CitrusParserStategy:
                    strategy = new CitrusParserStategy();
                    break;

                case ParseStategyEnum.Other:
                    strategy = new CitrusParserStategy();
                    break;

                default:
                    strategy = new CitrusParserStategy();
                    break;
            }

            _parser.SetStrategy(strategy);

            var items = _parser.GetAllItems(config.Path, config.AmountItems);//3) Polimorfism

            return items;
        }

        protected virtual void SaveParsedItems(IEnumerable<ParseResultItem> items)
        {
            foreach (ParseResultItem itemDto in items)
            {
                //use 'code' as unique id for items from one store 
                var storedItem = _uow.Items.FindFirst(e => e.Code == itemDto.Code);//TODO Refactor to make one request for all items

                if (storedItem == null)
                {
                    var item = _mapper.Map<Item>(itemDto);
                    item.Id = item.GetHashCode();

                    var price = ToPrice(itemDto, item);

                    _uow.Items.Create(item);
                    _uow.Prices.Create(price);
                }
                else
                {
                    var price = ToPrice(itemDto, storedItem);
                    _uow.Prices.Create(price);
                }
            }
            _uow.Save();
        }

        protected async virtual Task SaveParsedItemsAsync(IEnumerable<ParseResultItem> items)
        {
            foreach (ParseResultItem itemDto in items)
            {
                //use 'code' as unique id for items from one store 
                var storedItem = await _uow.Items.FindFirstAsync(e => e.Code == itemDto.Code);//TODO Refactor to make one request for all items

                if (storedItem == null)
                {
                    var item = _mapper.Map<Item>(itemDto);
                    item.Id = item.GetHashCode();

                    var price = ToPrice(itemDto, item);

                    await _uow.Items.CreateAsync(item);
                    await _uow.Prices.CreateAsync(price);
                }
                else
                {
                    var price = ToPrice(itemDto, storedItem);
                    await _uow.Prices.CreateAsync(price);
                }
            }
            _uow.Save();
        }
    }
}
