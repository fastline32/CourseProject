using Api;
using Api.Controllers;
using Infrastructure.DTOs;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using ShopProject.Test.Mocks;
using Xunit;

namespace ShopProject.Test.Controllers;

public abstract class CategoryTest : IDisposable
{
    private CategoryController instance { get; set; }

    protected CategoryController Test()
    {
        ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
        TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
        ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());
            
        var data = DatabaseMock.Instance;
        var mapper = MapperMock.Instance;
        var repo = new CategoryRepository(data);
        instance = new CategoryController(repo, mapper);
        instance.TempData = tempData;

        return instance;
    }
    public void Dispose()
    {
        instance.Dispose();
    }
}
public class CategoryControllerTest : CategoryTest
{
    
    [Fact]
    public void IndexReturnViewTest()
    {
        //Arrange
        
        var categoryController = Test();

        //Act
        var result = categoryController.Index();
        categoryController.Dispose();
        
        //Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public  void CreateReturnViw()
    {
        //Arrange
        var categoryController = Test();
        //Act
       
        var result =  categoryController.Create();
        categoryController.Dispose();
        
        //Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async void CreateReturnRedirectWithNull()
    {
        //Arrange
        var categoryController = Test();
        
        categoryController.ModelState.AddModelError("","test");
        
        //Act
        await categoryController.Create(new EntryCategoryViewModel {DisplayOrder = 2, Name = "Test"});
        var result = await categoryController.Create(null) as ViewResult;
        categoryController.Dispose();
        
        //Assert
        
        Assert.NotNull(result.TempData[WebConstants.Error]);
        Assert.IsType<ViewResult>(result);
        
    }
    
    [Fact]
    public async void CreateReturnRedirectWithSameName()
    {
        //Arrange
        var categoryController = Test();

        //Act
        var result1 = await categoryController.Create(new EntryCategoryViewModel {DisplayOrder = 2, Name = "Test"});
        var result = await categoryController.Create(new EntryCategoryViewModel {DisplayOrder = 3, Name = "Test"});
        
        
        //Assert
        Assert.IsType<RedirectToActionResult>(result1);
        Assert.IsType<ViewResult>(result);

    }

    [Fact]
    public async void EditTestMethodWithInvalidData()
    {
        //Arrange
        var controller = Test();
        
        //Act
        var result = await controller.Edit(0);
        var result1 = await controller.Edit(1);
        controller.Dispose();
        
        //Assert
        Assert.IsType<NotFoundResult>(result);
        Assert.IsType<NotFoundResult>(result1);
    }

    [Fact]
    public async void TestEditWithValidData()
    {
        //Arrange
        var controller = Test();
        
        //Act
        await controller.Create(new EntryCategoryViewModel {DisplayOrder = 1, Name = "Test"});
        var result = await controller.Edit(1);
        controller.Dispose();
        
        //Assert
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public async void EditPostTestMethod()
    {
        //Arrange
        
        var controller = Test();
        
        
        //Act
        await controller.Create(new EntryCategoryViewModel {DisplayOrder = 1, Name = "Test"});
        var result2 = await controller.Edit(1, new CategoryViewModel {DisplayOrder = 2, Id = 1, Name = "Test"});
        var result = await controller.Edit(2,new CategoryViewModel{DisplayOrder = 1,Id = 1,Name = "Test"});
        controller.ModelState.AddModelError("","Test");
        var result1 = await controller.Edit(1,new CategoryViewModel());
        
        controller.Dispose();
        
        //Assert
        Assert.IsType<RedirectToActionResult>(result2);
        Assert.IsType<NotFoundResult>(result);
        Assert.IsType<ViewResult>(result1);
    }
    
    [Fact]
    public async void DeleteTestMethod()
    {
        //Arrange
        var controller = Test();
        
        
        //Act
        await controller.Create(new EntryCategoryViewModel {DisplayOrder = 1, Name = "Test"});
        var result =await controller.Delete(1);
        
        controller.Dispose();
        
        //Assert
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public async void DeletePostTestMethod()
    {
        //Arrange
        var controller = Test();
        
        
        //Act
        await controller.Create(new EntryCategoryViewModel {DisplayOrder = 1, Name = "Test"});
        var result =await controller.DeleteConfirm(1);
        var result1 = await controller.DeleteConfirm(2);
        
        controller.Dispose();
        
        //Assert
        Assert.IsType<RedirectToActionResult>(result);
        Assert.IsType<NotFoundResult>(result1);
    }
}