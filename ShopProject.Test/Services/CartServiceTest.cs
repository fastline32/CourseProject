using Core.Data.EntryDbModels;
using Infrastructure.Repositories;
using Xunit;

namespace ShopProject.Test.Mocks;

public class CartServiceTest
{
    [Fact]
    public void IsUpdatedWithValidInput()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        
        var productRepo = new ProductRepository(data);
        productRepo.AddItemToDbAsync(new Product
        {
            Name = "Test",
            Description = "",
            CategoryId = 1,
            Id = 1,
            IsDeleted = false,
            ShortDescription = "nqma",
            Price = 10
        });
        //Act
        var cartService = new CartService(data);
        var item2 = new List<int> {1, 2};
        var result = cartService.GetAllProductsAsync(item2);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1,result.Count());
    }
}