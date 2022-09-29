using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Devon4Net.Test.xUnit.Test.Integration;
using Xunit;
using Devon4Net.Application.WebAPI.Implementation.Business.DishManagement.Service;
using Devon4Net.Domain.UnitOfWork.UnitOfWork;
using Devon4Net.Application.WebAPI.Implementation.Domain.Database;
using Devon4Net.Application.WebAPI.Implementation.Business.DishManagement.Controllers;
using Devon4Net.Application.WebAPI.Implementation.Business.DishManagement.Dto;
using Devon4Net.Application.WebAPI.Implementation.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Devon4Net.Application.WebAPI.Implementation.Business.DishManagement.Converters;
namespace Devon4Net.Test.xUnit.Test.UnitTest.Business
{
    public class DishControllerTest : UnitTest
    {
        /*
        [Fact]
        public async void DishSearch_Returns_Result()
        {
            //Arrange
            List<Dish> dishList = new List<Dish>();

            Dish dish1 = new Dish();
            dish1.Name = "burger";
            dish1.Price = 6;

            Dish dish2 = new Dish();
            dish2.Name = "pizza";
            dish2.Price = 8;

            Dish dish3 = new Dish();
            dish3.Name = "salad";
            dish3.Price = 5;
            dishList.Add(dish1);
            dishList.Add(dish2);
            dishList.Add(dish3);

            var dishServiceInterfaceMock = new Mock<IDishService>();
            dishServiceInterfaceMock.Setup(
                s => s.GetDishesMatchingCriterias(
                    It.IsAny<decimal>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<IList<long>>())
                ).Returns(
                    Task.FromResult(dishList)
                );

            DishController dishController = new DishController(dishServiceInterfaceMock.Object);
            FilterDtoSearchObjectDto input = new FilterDtoSearchObjectDto();


            input.SearchBy = "salad";
            input.MaxPrice = 10;
            input.MinLikes = 0;
            input.Categories = new CategorySearchDto[1];
            input.Categories[0] = new CategorySearchDto();
            input.Categories[0].Id = 1;

            var expectedResult = @"{""pagination"":{""size"":0,""page"":0,""total"":3},""content"":[{""dish"":{""id"":0,""modificationCounter"":0,""revision"":null,""name"":""burger"",""description"":null,""price"":6.0,""imageId"":null},""image"":{""id"":0,""modificationCounter"":null,""revision"":null,""name"":null,""content"":null,""contentType"":null,""mimeType"":null},""test"":null,""extras"":[],""categories"":[]},{""dish"":{""id"":0,""modificationCounter"":0,""revision"":null,""name"":""pizza"",""description"":null,""price"":8.0,""imageId"":null},""image"":{""id"":0,""modificationCounter"":null,""revision"":null,""name"":null,""content"":null,""contentType"":null,""mimeType"":null},""test"":null,""extras"":[],""categories"":[]},{""dish"":{""id"":0,""modificationCounter"":0,""revision"":null,""name"":""salad"",""description"":null,""price"":5.0,""imageId"":null},""image"":{""id"":0,""modificationCounter"":null,""revision"":null,""name"":null,""content"":null,""contentType"":null,""mimeType"":null},""test"":null,""extras"":[],""categories"":[]}]}";

            //Act
            var result = (ObjectResult)await dishController.DishSearch(input);

            //Assert
            dishServiceInterfaceMock.Verify(s => s.GetDishesMatchingCriterias(
                    It.IsAny<decimal>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<IList<long>>()), Times.Once());

            Assert.Equal(expectedResult, result.Value);
        }

        [Fact]
        public async void DishSearch_Null_Returns_Default()
        {
            //Arrange
            Dish dish1 = new Dish();
            dish1.Name = "burger";
            dish1.Price = 6;

            List<Dish> dishList = new List<Dish>();
            dishList.Add(dish1);

            var dishServiceInterfaceMock = new Mock<IDishService>();
            dishServiceInterfaceMock.Setup(
                s => s.GetDishesMatchingCriterias(
                    It.IsAny<decimal>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<IList<long>>())
                ).Returns(
                    Task.FromResult(dishList)
            );
            DishController dishController = new DishController(dishServiceInterfaceMock.Object);

            var expectedResult = @"{""pagination"":{""size"":0,""page"":0,""total"":1},""content"":[{""dish"":{""id"":0,""modificationCounter"":0,""revision"":null,""name"":""burger"",""description"":null,""price"":6.0,""imageId"":null},""image"":{""id"":0,""modificationCounter"":null,""revision"":null,""name"":null,""content"":null,""contentType"":null,""mimeType"":null},""test"":null,""extras"":[],""categories"":[]}]}";

            //Act
            var result = (ObjectResult)await dishController.DishSearch(null);
            //Assert
            dishServiceInterfaceMock.Verify(s => s.GetDishesMatchingCriterias(
                    It.IsAny<decimal>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<IList<long>>()), Times.Once());

            Assert.Equal(expectedResult, result.Value);
        }
        */
        private readonly Random rand = new();

        [Fact]
        public async Task DishSearch_WithGivenCategoryFilterCriteria_ReturnsAllCorrespondingDishes()
        {
            //Arrange
            var dishes = new[] { CreateRandomDish(), CreateRandomDish(), CreateRandomDish() };
            var randomPrice = (decimal)rand.NextDouble();
            var minLikes = rand.Next();
            var searchBy = Guid.NewGuid().ToString();

            List<long> categoryIds = new List<long>() { dishes[0].DishCategory.ToList().FirstOrDefault().Id, dishes[1].DishCategory.ToList().FirstOrDefault().Id };

            var categorySearchDtoIds = categoryIds.Select(c => new CategorySearchDto
            {
                Id = c,
            }).ToList().ToArray();

            List<Dish> expectedDishes = dishes.Where(dish => dish.DishCategory.Any(cat => categoryIds.Contains(cat.Id))).ToList();

            var serviceStub = new Mock<IDishService>();
            serviceStub.Setup(repo => repo.GetDishesMatchingCriterias(
                    //It.IsAny<decimal>(),
                    It.IsAny<decimal>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.Is<IList<long>>(categoryIdList => categoryIdList.All(cat => categoryIds.Contains(cat)))
                    )).Returns(Task.FromResult(expectedDishes));

            var controller = new DishController(serviceStub.Object);
            //Act
            var filterDto = new FilterDtoSearchObjectDto { MaxPrice = randomPrice, SearchBy = searchBy, MinLikes = minLikes, Categories = categorySearchDtoIds };
            var expectedResult = new ResultObjectDto<DishDtoResult> { };
            expectedResult.content = expectedDishes.Select(DishConverter.EntityToApi).ToList();
            expectedResult.Pagination.Total = expectedDishes.Count();

            var result = (ObjectResult)await controller.DishSearch(filterDto);

            //Assert
            var compareObj = JsonConvert.SerializeObject(expectedResult);
            Assert.Equal(compareObj, result.Value);
        }

        [Fact]
        public async Task DishSearch_WithGivenMaxPriceFilterCriteria_ReturnsAllCorrespondingDishes()
        {
            //Arrange
            var Dishes = new[] { CreateRandomDish(), CreateRandomDish(), CreateRandomDish() };
            var DishPrices = new[] { Dishes[0].Price, Dishes[1].Price, Dishes[2].Price };

            //Price Filter is set to max. Therefore all dishes will be taken into account.
            var expectedPrice = DishPrices.Max();
            var minLikes = rand.Next();
            var searchBy = Guid.NewGuid().ToString();
            var expectedCategories = Array.Empty<ICollection<Category>>();
            IList<long> categoryObjIds = new List<long>() { };
            var categorySearchDtoIds = categoryObjIds.Select(c => new CategorySearchDto
            {
                Id = c,
            }).ToList().ToArray();

            List<Dish> expectedDishes = Dishes.Where(dish => dish.Price <= expectedPrice).ToList();

            var serviceStub = new Mock<IDishService>();
            serviceStub.Setup(repo => repo.GetDishesMatchingCriterias(
                    //It.IsAny<decimal>(),
                    It.Is<decimal>(price => price == expectedPrice),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<IList<long>>())).Returns(Task.FromResult(expectedDishes));

            var controller = new DishController(serviceStub.Object);
            //Act
            var filterDto = new FilterDtoSearchObjectDto { MaxPrice = expectedPrice, SearchBy = searchBy, MinLikes = minLikes, Categories = categorySearchDtoIds };
            var expectedResult = new ResultObjectDto<DishDtoResult> { };
            expectedResult.content = expectedDishes.Select(DishConverter.EntityToApi).ToList();
            expectedResult.Pagination.Total = expectedDishes.Count();

            var result = (ObjectResult)await controller.DishSearch(filterDto);

            //Assert
            var compareObj = JsonConvert.SerializeObject(expectedResult);
            Assert.Equal(compareObj, result.Value);
        }

        
        [Fact]
        public async Task DishSearch_WithGivenMinPriceFilterCriteria_ReturnsAllCorrespondingDishes()
        {
            //Arrange
            var dishes = new[] { CreateRandomDish(), CreateRandomDish(), CreateRandomDish() };
            var dishPrices = new[] { dishes[0].Price, dishes[1].Price, dishes[2].Price };

            //Price Filter is set to min. Therefore only the cheapest dish(es) will be taken into account.
            var expectedPrice = dishPrices.Min();
            var minLikes = rand.Next();
            var searchBy = Guid.NewGuid().ToString();
            var expectedCategories = Array.Empty<ICollection<Category>>();
            IList<long> categoryObjIds = new List<long>() { };
            var categorySearchDtoIds = categoryObjIds.Select(c => new CategorySearchDto
            {
                Id = c,
            }).ToList().ToArray();

            List<Dish> expectedDishes = dishes.Where(dish => dish.Price <= expectedPrice).ToList();

            var serviceStub = new Mock<IDishService>();
            serviceStub.Setup(repo => repo.GetDishesMatchingCriterias(
                    //It.IsAny<decimal>(),
                    It.Is<decimal>(price => price == expectedPrice),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<IList<long>>())).Returns(Task.FromResult(expectedDishes));

            var controller = new DishController(serviceStub.Object);
            //Act
            var filterDto = new FilterDtoSearchObjectDto { MaxPrice = expectedPrice, SearchBy = searchBy, MinLikes = minLikes, Categories = categorySearchDtoIds };
            var expectedResult = new ResultObjectDto<DishDtoResult> { };
            expectedResult.content = expectedDishes.Select(DishConverter.EntityToApi).ToList();
            expectedResult.Pagination.Total = expectedDishes.Count();

            var result = (ObjectResult)await controller.DishSearch(filterDto);

            //Assert
            var compareObj = JsonConvert.SerializeObject(expectedResult);
            Assert.Equal(compareObj, result.Value);
        }

        [Fact]
        public async Task DishSearch_WithGivenSearchByCriteria_ReturnsAllCorrespondingDishes()
        {
            //Arrange
            var dishes = new[] { CreateRandomDish(), CreateRandomDish(), CreateRandomDish() };
            var expectedPrice = (decimal)rand.NextDouble();
            var minLikes = rand.Next();
            //Choose randomly one of the Dish names to filter for
            var dishNames = new[] { dishes[0].Name, dishes[1].Name, dishes[2].Name };
            var searchBy = dishNames[rand.Next(0, 3)];

            var expectedCategories = Array.Empty<ICollection<Category>>();
            IList<long> categoryObjIds = new List<long>() { };
            var categorySearchDtoIds = categoryObjIds.Select(c => new CategorySearchDto
            {
                Id = c,
            }).ToList().ToArray();

            List<Dish> expectedDishes = dishes.Where(dish => dish.Name.Contains(searchBy, StringComparison.CurrentCultureIgnoreCase)).ToList();

            var serviceStub = new Mock<IDishService>();
            serviceStub.Setup(repo => repo.GetDishesMatchingCriterias(
                    It.IsAny<decimal>(),
                    It.IsAny<int>(),
                    It.Is<string>(name => name == searchBy),
                    It.IsAny<IList<long>>())).Returns(Task.FromResult(expectedDishes));

            var controller = new DishController(serviceStub.Object);
            //Act
            var filterDto = new FilterDtoSearchObjectDto { MaxPrice = expectedPrice, SearchBy = searchBy, MinLikes = minLikes, Categories = categorySearchDtoIds };
            var expectedResult = new ResultObjectDto<DishDtoResult> { };
            expectedResult.content = expectedDishes.Select(DishConverter.EntityToApi).ToList();
            expectedResult.Pagination.Total = expectedDishes.Count();

            var result = (ObjectResult)await controller.DishSearch(filterDto);

            //Assert
            var compareObj = JsonConvert.SerializeObject(expectedResult);
            Assert.Equal(compareObj, result.Value);
        }


        [Fact]
        public async Task DishSearch_WithFullFilterCriteria_ReturnsAllCorrespondingDishes()
        {
            //Arrange
            var dishes = new[] { CreateRandomDish(), CreateRandomDish(), CreateRandomDish() };
            var dishPrices = new[] { dishes[0].Price, dishes[1].Price, dishes[2].Price };

            //Price Filter is set to max. Therefore all dishes will be taken into account.
            var expectedPrice = dishPrices.Max();
            var minLikes = rand.Next();

            //Choose randomly one of the Dish names to filter for
            var dishNames = new[] { dishes[0].Name, dishes[1].Name, dishes[2].Name };
            var searchBy = dishNames[rand.Next(0, 3)];

            List<long> categoryIds = new List<long>() { dishes[0].DishCategory.ToList().FirstOrDefault().Id, dishes[1].DishCategory.ToList().FirstOrDefault().Id };
            
            var categorySearchDtoIds = categoryIds.Select(c => new CategorySearchDto
            {
                Id = c,
            }).ToList().ToArray();

            List<Dish> expectedDishes = dishes.Where(dish =>
                dish.DishCategory.Any(cat => categoryIds.Contains(cat.Id)) &&
                dish.Price <= expectedPrice &&
                dish.Name.Contains(searchBy, StringComparison.CurrentCultureIgnoreCase)).ToList();

            var serviceStub = new Mock<IDishService>();
            serviceStub.Setup(repo => repo.GetDishesMatchingCriterias(
                    //It.IsAny<decimal>(),
                    It.Is<decimal>(price => price == expectedPrice),
                    It.Is<int>(likes => likes == minLikes),
                    It.Is<string>(searchCriteria => searchCriteria == searchBy),
                    It.Is<IList<long>>(categoryIdList => categoryIdList.All(cat => categoryIds.Contains(cat)))
                    )).Returns(Task.FromResult(expectedDishes));

            var controller = new DishController(serviceStub.Object);
            //Act

            var filterDto = new FilterDtoSearchObjectDto { MaxPrice = expectedPrice, SearchBy = searchBy, MinLikes = minLikes, Categories = categorySearchDtoIds };
            var expectedResult = new ResultObjectDto<DishDtoResult> { };
            expectedResult.content = expectedDishes.Select(DishConverter.EntityToApi).ToList();
            expectedResult.Pagination.Total = expectedDishes.Count();

            var result = (ObjectResult)await controller.DishSearch(filterDto);

            //Assert
            var compareObj = JsonConvert.SerializeObject(expectedResult);
            Assert.Equal(compareObj, result.Value);
        }

        
        [Fact]
        public async Task DishSearch_WithNullFilterCriteria_ReturnsDefault()
        {
            //Arrange
            List<Dish> Dishes = new List<Dish> { CreateRandomDish(), CreateRandomDish(), CreateRandomDish() };
            var serviceStub = new Mock<IDishService>();

            serviceStub.Setup(repo => repo.GetDishesMatchingCriterias(
                    It.IsAny<decimal>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<IList<long>>())).Returns(Task.FromResult(Dishes));

            var controller = new DishController(serviceStub.Object);
            //Act
            var expectedResult = new ResultObjectDto<DishDtoResult> { };
            expectedResult.content = Dishes.Select(DishConverter.EntityToApi).ToList();
            expectedResult.Pagination.Total = Dishes.Count();

            var result = (ObjectResult)await controller.DishSearch(null);

            //Assert
            var compareObj = JsonConvert.SerializeObject(expectedResult);
            Assert.Equal(compareObj, result.Value);
        }

        private DishCategory CreateRandomCategory()
        {
            return new()
            {
                Id = rand.NextInt64(),
            };
        }

        private Dish CreateRandomDish()
        {
            return new()
            {
                Id = rand.NextInt64(),
                Name = Guid.NewGuid().ToString(),
                Price = ((decimal)rand.NextDouble() + rand.Next()),
                DishCategory = new[] { CreateRandomCategory(), CreateRandomCategory(), CreateRandomCategory() }
            };
        }
    }
}
