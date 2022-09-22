using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using Devon4Net.Application.WebAPI.Implementation.Business.DishManagement.Service;
using Devon4Net.Domain.UnitOfWork.UnitOfWork;
using Devon4Net.Application.WebAPI.Implementation.Domain.Database;
using Devon4Net.Application.WebAPI.Implementation.Domain.Entities;
using Devon4Net.Application.WebAPI.Implementation.Domain.RepositoryInterfaces;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace Devon4Net.Test.xUnit.Test.UnitTest.Business
{
    public class DishServiceTest : UnitTest
    {
        [Fact]
        public async void getDishesMatchingCriterias_Returns_Price_Less_Than_6()
        {
            //Arrange
            IList<Dish> dishList = new List<Dish>();

            Dish dish1 = new Dish();
            dish1.Name = "falafel";
            dish1.Price = 6;

            Dish dish2 = new Dish();
            dish2.Name = "schnitzel";
            dish2.Price = 8;

            Dish dish3 = new Dish();
            dish3.Name = "salad";
            dish3.Price = 5;

            dishList.Add(dish1);
            dishList.Add(dish2);
            dishList.Add(dish3);

            var uowMock = new Mock<IUnitOfWork<ModelContext>>();
            var dishRepositoryMock = new Mock<IDishRepository>();
            dishRepositoryMock.Setup(
                repository => repository.GetAllNested(
                    It.IsAny<IList<String>>(),
                    It.IsAny<Expression<Func<Dish, bool>>>()
                )).Returns(
                    Task.FromResult(dishList)
                );
            uowMock.Setup(uow => uow.Repository<IDishRepository>()).Returns(dishRepositoryMock.Object);

            DishService dishService = new DishService(uowMock.Object);
            decimal maxPrice = 6;
            int minLikes = 0;
            string searchBy = "";
            IList<long> categoryIdList = new List<long>();

            var expectedList = new List<Dish>();
            expectedList.Add(dish3);
            var expectedResult = await Task.FromResult(expectedList);
            //Act
            var result = await dishService.GetDishesMatchingCriterias(maxPrice, minLikes, searchBy, categoryIdList);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async void getDishesMatchingCriterias_Where_CategoryId_Is_1()
        {
            //Arrange
            IList<Dish> dishList = new List<Dish>();
            DishCategory category1 = new DishCategory();
            category1.IdCategory = 1;
            DishCategory category2 = new DishCategory();
            category2.IdCategory = 2;

            Dish dish1 = new Dish();
            dish1.Name = "falafel";
            dish1.Price = 6;
            dish1.DishCategory.Add(category1);

            Dish dish2 = new Dish();
            dish2.Name = "schnitzel";
            dish2.Price = 8;
            dish2.DishCategory.Add(category2);

            Dish dish3 = new Dish();
            dish3.Name = "salad";
            dish3.Price = 5;
            dish3.DishCategory.Add(category1);
            dish3.DishCategory.Add(category2);

            dishList.Add(dish1);
            dishList.Add(dish2);
            dishList.Add(dish3);

            var uowMock = new Mock<IUnitOfWork<ModelContext>>();
            var dishRepositoryMock = new Mock<IDishRepository>();
            dishRepositoryMock.Setup(
                repository => repository.GetAllNested(
                    It.IsAny<IList<String>>(),
                    It.IsAny<Expression<Func<Dish, bool>>>()
                )).Returns(
                    Task.FromResult(dishList)
                );
            uowMock.Setup(uow => uow.Repository<IDishRepository>()).Returns(dishRepositoryMock.Object);

            DishService dishService = new DishService(uowMock.Object);
            decimal maxPrice = 0;
            int minLikes = 0;
            string searchBy = "";
            IList<long> categoryIdList = new List<long>();
            categoryIdList.Add(1);

            var expectedList = new List<Dish>();
            expectedList.Add(dish1);
            expectedList.Add(dish3);
            var expectedResult = await Task.FromResult(expectedList);
            //Act
            var result = await dishService.GetDishesMatchingCriterias(maxPrice, minLikes, searchBy, categoryIdList);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async void getDishesMatchingCriterias_SearchBy()
        {
            //Arrange
            IList<Dish> dishList = new List<Dish>();
            DishCategory category1 = new DishCategory();
            category1.IdCategory = 1;
            DishCategory category2 = new DishCategory();
            category2.IdCategory = 2;

            Dish dish1 = new Dish();
            dish1.Name = "falafel";
            dish1.Price = 6;
            dish1.DishCategory.Add(category1);

            Dish dish2 = new Dish();
            dish2.Name = "schnitzel";
            dish2.Price = 8;
            dish2.DishCategory.Add(category2);

            Dish dish3 = new Dish();
            dish3.Name = "salad";
            dish3.Price = 5;
            dish3.DishCategory.Add(category1);
            dish3.DishCategory.Add(category2);

            dishList.Add(dish1);
            dishList.Add(dish2);
            dishList.Add(dish3);

            var uowMock = new Mock<IUnitOfWork<ModelContext>>();
            var dishRepositoryMock = new Mock<IDishRepository>();
            dishRepositoryMock.Setup(
                repository => repository.GetAllNested(
                    It.IsAny<IList<String>>(),
                    It.IsAny<Expression<Func<Dish, bool>>>()
                )).Returns(
                    Task.FromResult(dishList)
                );
            uowMock.Setup(uow => uow.Repository<IDishRepository>()).Returns(dishRepositoryMock.Object);

            DishService dishService = new DishService(uowMock.Object);
            decimal maxPrice = 0;
            int minLikes = 0;
            string searchBy = "salad";
            IList<long> categoryIdList = new List<long>();


            var expectedList = new List<Dish>();
            expectedList.Add(dish3);

            var expectedResult = await Task.FromResult(expectedList);
            //Act
            var result = await dishService.GetDishesMatchingCriterias(maxPrice, minLikes, searchBy, categoryIdList);

            //Assert
            Assert.Equal(expectedResult, result);
        }
        [Fact]
        public async void getDishByIdTest()
        {
            //Arrange
            long searchedId = 1;

            Dish dish = new Dish();
            dish.Name = "falafel";
            dish.Price = 6;

            var uowMock = new Mock<IUnitOfWork<ModelContext>>();
            var dishRepositoryMock = new Mock<IDishRepository>();
            dishRepositoryMock.Setup(
                repository => repository.GetDishById(
                    It.IsAny<long>()
                )).Returns(
                    Task.FromResult(dish)
                );
            uowMock.Setup(uow => uow.Repository<IDishRepository>()).Returns(dishRepositoryMock.Object);

            DishService dishService = new DishService(uowMock.Object);

            //Act
            var result = await dishService.GetDishById(searchedId);

            //Assert
            dishRepositoryMock.Verify(s => s.GetDishById(
                    It.IsAny<long>()
                    ),
                    Times.Once());
            Assert.Equal(dish, result);

        }
    }
}
