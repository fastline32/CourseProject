using Api.Controllers;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.DTOs;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopProject.Test.Mocks;
using Xunit;

namespace ShopProject.Test.Controllers;

public class CategoryControllerTest
{
    [Fact]
    public void IndexReturnViewTest()
    {
        //Arrange
        var categoryController = new CategoryController(Mock.Of<ICategoryRepository>(),Mock.Of<IMapper>());
        //Act
        var result = categoryController.Index();
        
        //Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public  void CreateReturnViw()
    {
        //Arrange
        var categoryController = new CategoryController(Mock.Of<ICategoryRepository>(),Mock.Of<IMapper>());
        //Act
       
        var result =  categoryController.Create();
        
        //Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public async void CreateReturnRedirectWithInValidData()
    {
        var data = DatabaseMock.Instance;
        var mapper = MapperMock.Instance;
        var repo = new CategoryRepository(data);
        //Arrange
        var categoryController = new CategoryController(repo, mapper);
        categoryController.ModelState.AddModelError("","test");
        
        //Act
        var result = await categoryController.Create(new EntryCategoryViewModel{Name = "Test",DisplayOrder = 0});
        
        //Assert
        Assert.IsType<ViewResult>(result);
    }
}