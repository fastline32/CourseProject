
using Infrastructure.Repositories;
using ShopProject.Test.Mocks;
using Xunit;
using Type = Core.Data.EntryDbModels.Type;

namespace ShopProject.Test.Services;

public class TypeRepositoryTests
{
    [Fact]
    public void IsUpdatedWithValidInput()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        data.Types.Add(
            new Type()
            {
                Id = 1,
                IsDeleted = false,
                Name = "Test"
            });
        var typeRepository = new TypeRepository(data);
        
        //Act
        var newType = new Type()
        {
            Id = 1,
            IsDeleted = true,
            Name = "Test"
        };

        var result = typeRepository.UpdateAsync(newType);
        
        //Assert
        Assert.True(result.IsCompleted);
    }
    [Fact]
    public void TestFindByNameWithInvalidData()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        data.Types.Add(
            new Type()
            {
                Id = 1,
                IsDeleted = false,
                Name = "Test"
            });
        var typeRepository = new TypeRepository(data);
        
        //Act
        var result = typeRepository.FindByNameAsync("Test2");
        
        //Assert
        Assert.False(result);
    }
    [Fact]
    public void TestFindByNameWithValidData()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        data.Types.Add(
            new Type()
            {
                Id = 1,
                IsDeleted = false,
                Name = "Test"
            });
        var typeRepository = new TypeRepository(data);
        
        //Act
        var result = typeRepository.FindByNameAsync("Test");
        
        //Assert
        Assert.IsType<bool>(result);
    }
    
    [Fact]
    public async Task TestGetAll()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        data.Types.Add(
            new Type()
            {
                Id = 1,
                IsDeleted = false,
                Name = "Test"
            });
        var typeRepository = new TypeRepository(data);
        
        //Act
        var result =await typeRepository.GetAllAsync();
        
        //Assert
        Assert.IsType<List<Type>>(result);
    }
    [Fact]
    public async Task TestGetById()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        var newType = new Type()
        {
            Id = 1,
            IsDeleted = false,
            Name = "Test"
        };
        
        var typeRepository = new TypeRepository(data);
        var testResult =typeRepository.AddItemToDbAsync(newType);
        
        //Act
        var result =await typeRepository.GetByIdAsync(1);
        
        //Assert
        Assert.NotNull(testResult);
        Assert.Equal(1,data.Types.Count());
        Assert.NotNull(result);
        Assert.IsType<Type>(result);
    }
    
    [Fact]
    public async Task TestGetByName()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        var newType = new Type()
        {
            Id = 1,
            IsDeleted = false,
            Name = "Test"
        };
        
        var typeRepository = new TypeRepository(data);
        await typeRepository.AddItemToDbAsync(newType);
        
        //Act
        var result =await typeRepository.GetByNameAsync("Test");
        
        //Assert
        Assert.NotNull(result);
        Assert.IsType<Type>(result);
    }
    
    [Fact]
    public async void TestGetSelectedList()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        var newType = new Type()
        {
            Id = 1,
            IsDeleted = false,
            Name = "Test"
        };
        var newType2 = new Type()
        {
            Id = 2,
            IsDeleted = false,
            Name = "Test 2"
        };
        
        var typeRepository = new TypeRepository(data);
        await typeRepository.AddItemToDbAsync(newType);
        await typeRepository.AddItemToDbAsync(newType2);
        
        //Act
        var result = typeRepository.GetSelectListAsync();
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(2,result.Count());
    }
}