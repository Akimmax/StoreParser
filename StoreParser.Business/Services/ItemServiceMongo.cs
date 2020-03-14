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
    public class ItemServiceMongo: ItemService
    {
        public ItemServiceMongo(IUnitOfWork uow, Parser parser, IMapper mapper): base( uow,  parser,  mapper)
        {
        }

        protected override void SaveNewItem(ParseResultItem itemDto)
        {
            var item = _mapper.Map<Item>(itemDto);
            item.Id = item.GetHashCode();

            item.Prices.Add(new Price
            {
                CurrentPrice = itemDto.Price,
                Date = DateTime.Now
            });

            _uow.Items.Create(item);
        }

        protected override void UpdateExistingItem(ParseResultItem itemDto, Item storedItem)
        {
            storedItem.Prices.Add(new Price
            {
                CurrentPrice = itemDto.Price,
                Date = DateTime.Now
            });
            _uow.Items.Create(storedItem);
        }
    }
}

