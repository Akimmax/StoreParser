﻿using StoreParser.Data;
using System;
using System.Linq;
using StoreParser.Dtos;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace StoreParser.Business
{
    public interface IParseService
    {
        IEnumerable<ItemDto> GetAll();
        IEnumerable<NewItemDto> AddItems(ParsingConfiguration config);
        ItemDto GetById(int id);

        Task<IEnumerable<ItemDto>> GetAllAsync();
        Task<IEnumerable<NewItemDto>> AddItemsAsync(ParsingConfiguration config);
        Task<ItemDto> GetByIdAsync(int id);
    }
}
