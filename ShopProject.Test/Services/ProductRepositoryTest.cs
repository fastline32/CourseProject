using Core.Data.EntryDbModels;
using Infrastructure.Repositories;
using ShopProject.Test.Mocks;
using Xunit;

namespace ShopProject.Test.Services;


public class ProductRepositoryTest
{
    [Fact]
    public async void GetAllTestMethod()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        
        var productRepository = new ProductRepository(data);
        
        //Act
        await productRepository.AddItemToDbAsync(new Product
        {
            Name = "Test",
            Description = "",
            CategoryId = 1,
            Id = 1,
            IsDeleted = false,
            ShortDescription = "nqma",
            Price = 10
        });
        await productRepository.AddItemToDbAsync(new Product
        {
            Name = "Test2",
            Description = "",
            CategoryId = 1,
            Id = 2,
            IsDeleted = false,
            ShortDescription = "nqma",
            Price = 11
        });

        var result =await productRepository.GetAllAsync();
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(2,data.Products.Count());
        Assert.IsType<List<Product>>(result);
    }
    
    [Fact]
    public async void FindByNameTestMethod()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        
        var productRepository = new ProductRepository(data);
        
        //Act
        await productRepository.AddItemToDbAsync(new Product
        {
            Name = "Test",
            Description = "",
            CategoryId = 1,
            Id = 1,
            IsDeleted = false,
            ShortDescription = "nqma",
            Price = 10
        });
        await productRepository.AddItemToDbAsync(new Product
        {
            Name = "Test2",
            Description = "",
            CategoryId = 1,
            Id = 2,
            IsDeleted = false,
            ShortDescription = "nqma",
            Price = 11
        });

        var result = productRepository.FindByNameAsync("Test2");
        var result2 = productRepository.FindByNameAsync("Test3");
        
        //Assert
        Assert.NotNull(result);
        Assert.True(result2 == false);
        Assert.True(result == true);
    }
    
    
    [Fact]
    public async Task GetByNameTestMethod()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        
        var productRepository = new ProductRepository(data);
        
        //Act
        await productRepository.AddItemToDbAsync(new Product
        {
            Name = "Test",
            Description = "",
            CategoryId = 1,
            Id = 1,
            IsDeleted = false,
            ShortDescription = "nqma",
            Price = 10
        });
        await productRepository.AddItemToDbAsync(new Product
        {
            Name = "Test2",
            Description = "",
            CategoryId = 1,
            Id = 2,
            IsDeleted = false,
            ShortDescription = "nqma",
            Price = 11
        });

        var testProduct = await productRepository.GetByNameAsync("Test2");
        
        //Assert
        Assert.NotNull(testProduct);
        Assert.IsType<Product>(testProduct);
    }
    
    // public async Task GetByIdMethodTest()
    // {
    //     //Arrange
    //     var data = DatabaseMock.Instance;
    //     
    //     var productRepository = new ProductRepository(data);
    //     
    //     //Act
    //     await productRepository.AddItemToDbAsync(new Product
    //     {
    //         Name = "Test",
    //         Description = "",
    //         CategoryId = 1,
    //         Id = 1,
    //         IsDeleted = false,
    //         ShortDescription = "nqma",
    //         TypeId = 1,
    //         Price = 10
    //     });
    //     await data.SaveChangesAsync();
    //     
    //     //Assert
    //     var result = await productRepository.GetByIdAsync(1);
    //     Assert.NotNull(result);
    //     Assert.IsType<Product>(result);
    }
    
    // [Fact]
    // public async Task UpdateMethodTest()
    // {
    //     //Arrange
    //     var data = DatabaseMock.Instance;
    //     
    //     var productRepository = new ProductRepository(data);
    //     
    //     //Act
    //     await productRepository.AddItemToDbAsync(new Product
    //     {
    //         Name = "Test",
    //         Description = "",
    //         CategoryId = 1,
    //         Id = 1,
    //         IsDeleted = false,
    //         ShortDescription = "nqma",
    //         Price = 10
    //     });
    //     await productRepository.AddItemToDbAsync(new Product
    //     {
    //         Name = "Test2",
    //         Description = "",
    //         CategoryId = 1,
    //         Id = 2,
    //         IsDeleted = false,
    //         ShortDescription = "nqma",
    //         Price = 11
    //     });
    //     await data.SaveChangesAsync();
    //
    //     var newProduct = new Product
    //     {
    //         Name = "Test3",
    //         Description = "",
    //         CategoryId = 1,
    //         Id = 2,
    //         IsDeleted = false,
    //         ShortDescription = "nqma",
    //         Price = 11
    //     };
    //     productRepository.Update(newProduct);
    //     await data.SaveChangesAsync();
    //
    //     var testProduct =await productRepository.GetByIdAsync(2);
    //     //Assert
    //     Assert.NotNull(testProduct);
    //     Assert.Equal("Test3", testProduct.Name);
    // }