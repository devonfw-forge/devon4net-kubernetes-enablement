using Moq;
using System;
using System.Collections.Generic;
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


            /*
            string expectedResult = @"{
                'pagination':{'size':0,'page':0,'total':0},'content':[]}";
            */

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

    }
}
