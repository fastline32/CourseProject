using Api.Controllers;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.DTOs;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopProject.Test.Mocks;
using Xunit;
using Type = Core.Data.EntryDbModels.Type;

namespace ShopProject.Test.Controllers;

public class TypeControllerTest
{
    [Fact]
    public async void IndexReturnView()
    {
        //Arrange
        var typeController = new TypeController(Mock.Of<IMapper>(),Mock.Of<ITypeRepository>());
        //Act
        var result =await typeController.Index();
        
        //Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public async void CreateReturnRedirectWithValidData()
    {
        //Arrange
        var typeController = new TypeController(Mock.Of<IMapper>(),Mock.Of<ITypeRepository>());
        //Act
        var model = new EntryTypeModel
        {
            IsDeleted = false,
            Name = "Test"
        };
        var result = await typeController.Create(model);
        
        //Assert
        Assert.NotNull(result);
        Assert.IsType<RedirectToActionResult>(result);
    }
    
    [Fact]
    public async void CreateReturnRedirectWithInValidData()
    {
        //Arrange
        var typeController = new TypeController(Mock.Of<IMapper>(), Mock.Of<ITypeRepository>());
        typeController.ModelState.AddModelError("","test");
        
        //Act
        var result = await typeController.Create(new EntryTypeModel {IsDeleted = false, Name = null});
        
        //Assert
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public async void CreateReturnRedirectWithSameData()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        var repo = new TypeRepository(data);
        var typeController = new TypeController(Mock.Of<IMapper>(), repo);
        await repo.AddItemToDbAsync(new Type {IsDeleted = false, Name = "Test"});
        
        Assert.Equal(1,data.Types.Count());
        
        //Act
        var result = await typeController.Create(new EntryTypeModel {IsDeleted = false, Name = "Test"});
        
        //Assert
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public  void CreateReturnViw()
    {
        //Arrange
        var typeController = new TypeController(Mock.Of<IMapper>(),Mock.Of<ITypeRepository>());
        //Act
       
        var result =  typeController.Create();
        
        //Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public async void EditReturnViw()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        var repo = new TypeRepository(data);
        var typeController = new TypeController(Mock.Of<IMapper>(), repo);
        await repo.AddItemToDbAsync(new Type {IsDeleted = false, Name = "Test"});
        //Act
       
        var result =await typeController.Edit(1);
        var result2 = await typeController.Edit(3);
        var result3 = await typeController.Edit(0);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
        Assert.NotNull(result2);
        Assert.IsType<NotFoundResult>(result2);
        Assert.NotNull(result3);
        Assert.IsType<NotFoundResult>(result3);
    }
    
    [Fact]
    public async void EditPostReturnViw()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        var repo = new TypeRepository(data);
        var mapper = MapperMock.Instance;
        var typeController = new TypeController(mapper, repo);
        await repo.AddItemToDbAsync(new Type {IsDeleted = false, Name = "Test"});
        //Act
        var item = new TypeViewModel
        {
            Name = "Test2",
            IsDeleted = false
        };
        
        var result =await typeController.Edit(1,item);
        var result2 =await typeController.Edit(0,item);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<RedirectToActionResult>(result);
        
        Assert.NotNull(result2);
        Assert.IsType<NotFoundResult>(result2);
        // Assert.NotNull(result3);
        // Assert.IsType<NotFoundResult>(result3);
    }
    
    [Fact]
    public async void EditReturnWithInValidData()
    {
        //Arrange
        var typeController = new TypeController(Mock.Of<IMapper>(), Mock.Of<ITypeRepository>());
        typeController.ModelState.AddModelError("","test");
        
        //Act
        var result = await typeController.Edit(1,new TypeViewModel() {IsDeleted = false, Name = null});
        
        //Assert
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public async void DeleteReturnViw()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        var repo = new TypeRepository(data);
        var mapper = MapperMock.Instance;
        var typeController = new TypeController(mapper, repo);
        await repo.AddItemToDbAsync(new Type {IsDeleted = false, Name = "Test"});
        //Act
       
        var result =await typeController.Delete(1);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public async void DeletePostReturnViw()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        var repo = new TypeRepository(data);
        var mapper = MapperMock.Instance;
        var typeController = new TypeController(mapper, repo);
        await repo.AddItemToDbAsync(new Type {IsDeleted = false, Name = "Test"});
        //Act
        var item =await repo.GetByNameAsync("Test");
        var intNum = item.Id;
        var result =await typeController.DeleteConfirm(intNum);
        var result2 =await typeController.DeleteConfirm(0);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<RedirectToActionResult>(result);
        
        Assert.NotNull(result2);
        Assert.IsType<NotFoundResult>(result2);
    }
}