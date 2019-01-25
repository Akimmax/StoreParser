using AutoMapper;
using AutoMapper.Configuration;
using StoreParser.Data;
using StoreParser.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreParser.Business
{
    public class MappingProfile : MapperConfigurationExpression
    {
        public MappingProfile()
        {
            CreateMap<ParseResultItem, NewItemDto>();

            CreateMap<ParseResultItem, Item>()
            .ForMember("Prices", opt => opt.MapFrom(src => new List<Price>()));

            CreateMap<Item, ItemDto>()
            .ForMember("Prices", opt => opt.MapFrom(src => new List<Price>()));
        }
    }
}
