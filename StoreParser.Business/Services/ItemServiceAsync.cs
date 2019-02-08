using StoreParser.Data;
using System;
using System.Linq;
using StoreParser.Dtos;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace StoreParser.Business
{
    public class ItemServiceAsync : IParseService
    { 
        Parser _parser;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ItemServiceAsync(IUnitOfWork uow, Parser parser, IMapper mapper)
        {
            _uow = uow;
            _parser = parser;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NewItemDto>> AddItemsAsync(ParsingConfiguration config)
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

            var newRecords = _mapper.Map<IEnumerable<NewItemDto>>(items);

            foreach (ParseResultItem i in items)
            {
                Expression<Func<Item, bool>> FilterByCode(string code)
                {
                    return x => x.Code == code;
                }

                var storedItem = await _uow.ItemsAsync.FindFirstAsync(FilterByCode(i.Code));//use 'code' as unique id for items from one store

                if (storedItem == null)
                {
                    var item = _mapper.Map<Item>(i);

                    var price = new Price
                    {                        
                        Item = item,
                        CurrentPrice = i.Price,
                        Date = DateTime.Now
                    };

                    await  _uow.ItemsAsync.CreateAsync(item);
                    await  _uow.PricesAsync.CreateAsync(price);
                }
                else
                {
                    var price = new Price
                    {
                        Item = storedItem,
                        CurrentPrice = i.Price,
                        Date = DateTime.Now
                    };
                    await  _uow.PricesAsync.CreateAsync(price);
                }
            }
            _uow.Save();

            return newRecords;
        }

        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {
            IList<ItemDto> list = new List<ItemDto>();

            var items = await  _uow.ItemsAsync.GetAllAsync();

            foreach (var item in items)
            {
                ItemDto itemDto = _mapper.Map<ItemDto>(item);
                itemDto.Prices = item.Prices.Select(pr => new PriceDto { Date = pr.Date, Price = pr.CurrentPrice });

                list.Add(itemDto);
            }

            return list;
        }

        public async Task<ItemDto> GetByIdAsync(int id)
        {
            var item = await _uow.ItemsAsync.FindFirstAsync(e => e.Id == id);

            ItemDto itemDto = _mapper.Map<ItemDto>(item);
            itemDto.Prices = item.Prices.Select(pr => new PriceDto { Date = pr.Date, Price = pr.CurrentPrice });

            return itemDto;
        }

        public IEnumerable<ItemDto> GetAll() { throw new NotImplementedException(); }
        public IEnumerable<NewItemDto> AddItems(ParsingConfiguration config) { throw new NotImplementedException(); }
        public ItemDto GetById(int id) { throw new NotImplementedException(); }

    }
}
