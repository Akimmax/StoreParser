using StoreParser.Data;
using System;
using System.Linq;
using StoreParser.Dtos;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

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



            foreach (ParseResultItem itemDto in items)
            {
                //use 'code' as unique id for items from one store 
                var storedItem = _uow.Items.FindFirst(e => e.Code == itemDto.Code);//TODO Refactor to make one request for all items


                if (storedItem == null)
                {
                    SaveNewItem(itemDto);
                }
                else
                {
                    UpdateExistingItem(itemDto, storedItem);
                }
            }
            _uow.Save();

            return newRecords;
        }

        public IEnumerable<ItemDto> GetAll()
        {
            IList<ItemDto> list = new List<ItemDto>();

            var items = _uow.Items.GetAll();            

            foreach (var item in items)
            {
                ItemDto itemDto = _mapper.Map<ItemDto>(item);
                itemDto.Prices = item.Prices.Select(pr => new PriceDto { Date = pr.Date, Price = pr.CurrentPrice });

                list.Add(itemDto);
            }

            return list;
        }

        public ItemDto GetById(int id)
        {           
            var item = _uow.Items.Get(id);

            ItemDto itemDto = _mapper.Map<ItemDto>(item);
            itemDto.Prices = item.Prices.Select(pr => new PriceDto { Date = pr.Date, Price = pr.CurrentPrice });

            return itemDto;
        }

        protected virtual void SaveNewItem(ParseResultItem itemDto)
        {
            var item = _mapper.Map<Item>(itemDto);
            item.Id = item.GetHashCode();

            var price = new Price
            {
                Item = item,
                CurrentPrice = itemDto.Price,
                Date = DateTime.Now
            };

            _uow.Items.Create(item);
            _uow.Prices.Create(price);
        }

        protected virtual void UpdateExistingItem(ParseResultItem itemDto, Item storedItem)
        {
            var price = new Price
            {
                Item = storedItem,
                CurrentPrice = itemDto.Price,
                Date = DateTime.Now
            };
            _uow.Prices.Create(price);
        }

    }
}
