using StoreParser.Data;
using System;
using System.Linq;
using StoreParser.Dtos;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using StoreParser.Data.Repositories;

namespace StoreParser.Business
{
    public class ItemServiceMongo : IParseService
    { 
        Parser _parser;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private MongoContext mongoContext;
        private MongoRep rep;
        public ItemServiceMongo(IUnitOfWork uow, Parser parser, IMapper mapper)
        {
            _uow = uow;
            _parser = parser;
            _mapper = mapper;

            mongoContext = new MongoContext("");
            rep = new Data.Repositories.MongoRep(mongoContext);
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

            foreach (ParseResultItem i in items)
            {
                var storedItem = _uow.Items.FindFirst(e => e.Code == i.Code);//use 'code' as unique id for items from one store

                if (storedItem == null)
                {
                    var item = _mapper.Map<Item>(i);
                    item.Id = item.GetHashCode();

                    item.Prices.Add(new Price
                    {
                        CurrentPrice = i.Price,
                        Date = DateTime.Now
                    });

                    _uow.Items.Create(item);
                }
                else
                {

                    storedItem.Prices.Add(new Price
                    {
                        CurrentPrice = i.Price,
                        Date = DateTime.Now
                    });
                    _uow.Items.Create(storedItem);
                }
            }
            _uow.Save();

            return newRecords;
        }

        public IEnumerable<ItemDto> GetAll()
        {
            //IList<ItemDto> list = new List<ItemDto>();

            //var items = _uow.Items.GetAll();

            //foreach (var item in items)
            //{
            //    ItemDto itemDto = _mapper.Map<ItemDto>(item);
            //    itemDto.Prices = item.Prices.Select(pr => new PriceDto { Date = pr.Date, Price = pr.CurrentPrice });

            //    list.Add(itemDto);
            //}


            IList<ItemDto> list = new List<ItemDto>();

            var items = rep.GetAll();

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

    }
}
