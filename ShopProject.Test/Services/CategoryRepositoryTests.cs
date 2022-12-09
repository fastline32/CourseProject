using Core.Data.EntryDbModels;
using Infrastructure.Repositories;
using ShopProject.Test.Mocks;
using Xunit;

namespace ShopProject.Test.Services;

public class CategoryRepositoryTests
{
    [Fact]
    public void IsUpdatedWithValidInput()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        data.Categories.Add(
            new Category
            {
                DisplayOrder = 2,
                Id = 1,
                IsDeleted = false,
                Name = "Test"
            });
        var categoryRepository = new CategoryRepository(data);
        
        //Act
        var newCategory = new Category
        {
            DisplayOrder = 3,
            Id = 1,
            IsDeleted = false,
            Name = "Test"
        };

        var result = categoryRepository.Update(newCategory);
        
        //Assert
        Assert.True(result.IsCompleted);
    }
    [Fact]
    public void IsUpdatedWithInValidInput()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        data.Categories.Add(
            new Category
            {
                DisplayOrder = 2,
                Id = 1,
                IsDeleted = false,
                Name = "Test"
            });
        var categoryRepository = new CategoryRepository(data);
        
        //Act
        var newCategory = new Category
        {
            DisplayOrder = 3,
            Id = 2,
            IsDeleted = false,
            Name = "Test"
        };

        var result = categoryRepository.Update(newCategory);
        
        //Assert
        Assert.False(result.IsFaulted);
    }
}