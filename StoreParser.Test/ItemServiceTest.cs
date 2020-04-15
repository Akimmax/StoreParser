using StoreParser.Data;
using System;
using Xunit;

using System.Linq;
using System.Threading.Tasks;
using StoreParser.Dtos;
using StoreParser.Business;
using Moq;

using System.Linq.Expressions;
using AutoMapper;
using System.Collections.Generic;

namespace StoreParser.Test
{
    public class ItemServiceTest
    {
        [Fact]
        public void Should_Get_All_Items()
        {
            var repositoryMock = new Mock<IRepository<Item>>(MockBehavior.Default);
            repositoryMock.Setup(m => m.GetAll()).Returns(() => new[]
            {
                new Item { Id=0, Description ="MyItem1"  },
                new Item { Id=1, Description ="MyItem2"  },
                new Item { Id=2, Description ="MyItem3"  },
            });

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Items).Returns(() => repositoryMock.Object);

            var mappingProfile = new MappingProfile();
            var config = new MapperConfiguration(mappingProfile);
            var mapper = new Mapper(config);

            var service = new ItemService(unitOfWorkMock.Object, new Parser(), mapper);

            var items = service.GetAll().ToArray();

            Assert.Equal(3, items.Count());
            Assert.Equal("MyItem1", items[0].Description);
            repositoryMock.Verify(m => m.GetAll());
        }

        [Fact]
        public void Should_Add_New_Items()
        {
            var itemGet = new Item { Id = 1, Description = "MyItem1" };

            var resuattItems = new[]
            {
                new ParseResultItem {Description = "MyItem1", Price = 10, Code = "1"  },
                new ParseResultItem {Description = "MyItem2", Price = 10, Code = "2"  },
                new ParseResultItem {Description = "MyItem3", Price = 10, Code = "3" },
            };

            var parsingConfiguration = new ParsingConfiguration
            {
                Path = "https://www.citrus.ua/bluetooth-garnitury/",
                Stategy = ParseStategyEnum.CitrusParserStategy,
                AmountItems = 10
            };

            var parserMock = new Mock<Parser>();
            parserMock.Setup(u => u.GetAllItems(parsingConfiguration.Path, parsingConfiguration.AmountItems)).Returns(resuattItems);

            var repositoryMock = new Mock<IRepository<Item>>(MockBehavior.Default);
            var repositorypriseMock = new Mock<IRepository<Price>>(MockBehavior.Default);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Items).Returns(() => repositoryMock.Object);
            unitOfWorkMock.Setup(u => u.Prices).Returns(() => repositorypriseMock.Object);

            var mappingProfile = new MappingProfile();
            var config = new MapperConfiguration(mappingProfile);
            var mapper = new Mapper(config);

            var service = new ItemService(unitOfWorkMock.Object, parserMock.Object, mapper);

            service.AddItems(parsingConfiguration);

            repositoryMock.Verify(m => m.Create(It.Is<Item>(t => t.Code == resuattItems[0].Code)));
            repositoryMock.Verify(m => m.Create(It.Is<Item>(t => t.Code == resuattItems[1].Code)));
            repositoryMock.Verify(m => m.Create(It.Is<Item>(t => t.Code == resuattItems[2].Code)));

            unitOfWorkMock.Verify(m => m.Save());
        }

        [Fact]
        public void Should_Get_Item_ById()
        {
            var itemGet = new Item { Id = 1, Description = "MyItem1" };

            var repositoryMock = new Mock<IRepository<Item>>();
            repositoryMock.Setup(u => u.Get(1)).Returns(itemGet);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Items).Returns(() => repositoryMock.Object);

            var mappingProfile = new MappingProfile();
            var config = new MapperConfiguration(mappingProfile);
            var mapper = new Mapper(config);

            var service = new ItemService(unitOfWorkMock.Object, new Parser(), mapper);

            var actualGet = service.GetById(1);

            Assert.NotNull(actualGet);
            Assert.Equal(itemGet.Description, itemGet.Description);

            repositoryMock.Verify(m => m.Get(1));
        }

        [Fact]
        public async void Should_Get_All_ItemsAsync()
        {
            var repositoryMock = new Mock<IRepository<Item>>(MockBehavior.Default);

            repositoryMock.Setup(m => m.GetAllAsync()).Returns(Task<IEnumerable < Item>>
                .Factory
                .StartNew(() => new[]
            {
                new Item { Id=0, Description ="MyItem1Async"  },
                new Item { Id=1, Description ="MyItem2Async"  },
                new Item { Id=2, Description ="MyItem3Async"  },
            }));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Items).Returns(() => repositoryMock.Object);

            var mappingProfile = new MappingProfile();
            var config = new MapperConfiguration(mappingProfile);
            var mapper = new Mapper(config);

            var service = new ItemService(unitOfWorkMock.Object, new Parser(), mapper);

            var items = await service.GetAllAsync();
            var itemsArray = items.ToArray();


            Assert.Equal(3, itemsArray.Count());
            Assert.Equal("MyItem1Async", itemsArray[0].Description);
            repositoryMock.Verify(m => m.GetAllAsync());
        }

        [Fact]
        public async void Should_Add_New_ItemsAsync()
        {
            var itemGet = new Item { Id = 1, Description = "MyItem1Async" };

            var resuattItems = new[]
            {
                new ParseResultItem {Description = "MyItem1Async", Price = 10, Code = "1"  },
                new ParseResultItem {Description = "MyItem2Async", Price = 10, Code = "2"  },
                new ParseResultItem {Description = "MyItem3Async", Price = 10, Code = "3" },
            };

            var parsingConfiguration = new ParsingConfiguration
            {
                Path = "https://www.citrus.ua/bluetooth-garnitury/",
                Stategy = ParseStategyEnum.CitrusParserStategy,
                AmountItems = 10
            };

            var parserMock = new Mock<Parser>();
            parserMock.Setup(u => u.GetAllItems(parsingConfiguration.Path, parsingConfiguration.AmountItems)).Returns(resuattItems);

            var repositoryMock = new Mock<IRepository<Item>>(MockBehavior.Default);
            var repositorypriseMock = new Mock<IRepository<Price>>(MockBehavior.Default);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Items).Returns(() => repositoryMock.Object);
            unitOfWorkMock.Setup(u => u.Prices).Returns(() => repositorypriseMock.Object);

            var mappingProfile = new MappingProfile();
            var config = new MapperConfiguration(mappingProfile);
            var mapper = new Mapper(config);

            var service = new ItemService(unitOfWorkMock.Object, parserMock.Object, mapper);

            await service.AddItemsAsync(parsingConfiguration);

            repositoryMock.Verify(m => m.CreateAsync(It.Is<Item>(t => t.Code == resuattItems[0].Code)));
            repositoryMock.Verify(m => m.CreateAsync(It.Is<Item>(t => t.Code == resuattItems[1].Code)));
            repositoryMock.Verify(m => m.CreateAsync(It.Is<Item>(t => t.Code == resuattItems[2].Code)));

            unitOfWorkMock.Verify(m => m.Save());
        }

        [Fact]
        public async void Should_Get_Item_ByIdAsync()
        {
            var itemGet = new Item { Id = 1, Description = "MyItem1Async" };

            var repositoryMock = new Mock<IRepository<Item>>();
            repositoryMock.Setup(u => u.GetAsync(1)).Returns(Task<Item>
                .Factory
                .StartNew(() => itemGet));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Items).Returns(() => repositoryMock.Object);

            var mappingProfile = new MappingProfile();
            var config = new MapperConfiguration(mappingProfile);
            var mapper = new Mapper(config);

            var service = new ItemService(unitOfWorkMock.Object, new Parser(), mapper);

            var actualGet = await service.GetByIdAsync(1);

            Assert.NotNull(actualGet);
            Assert.Equal(itemGet.Description, itemGet.Description);

            repositoryMock.Verify(m => m.GetAsync(1));
        }
    }
}
