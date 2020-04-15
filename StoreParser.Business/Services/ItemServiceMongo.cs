using StoreParser.Data;
using System;
using System.Linq;
using StoreParser.Dtos;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using StoreParser.Data.Repositories;
using System.Threading.Tasks;

namespace StoreParser.Business
{
    public class ItemServiceMongo: ItemService
    {
        public ItemServiceMongo(IUnitOfWork uow, Parser parser, IMapper mapper): base( uow,  parser,  mapper)
        {
        }

        protected override void SaveParsedItems(IEnumerable<ParseResultItem> items)
        {
            //TODO Refactor to use Bulk oparation (CreateAll )
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
                }
                else
                {
                    _uow.Items.Create(storedItem);
                }
            }
            _uow.Save();
        }


        protected override async Task SaveParsedItemsAsync(IEnumerable<ParseResultItem> items)
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
                }
                else
                {
                    await _uow.Items.CreateAsync(storedItem);
                }
            }
            _uow.Save();
        }
    }
}

