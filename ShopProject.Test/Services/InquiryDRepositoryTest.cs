using Core.Data.EntryDbModels.Inquiry;
using Infrastructure.Repositories;
using ShopProject.Test.Mocks;
using Xunit;

namespace ShopProject.Test.Services;

public class InquiryDRepositoryTest
{
    [Fact]
    public async void GetAllTestMethod()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        var inquiryDRepository = new InquiryDetailsRepository(data);
        
        //Act
        data.InquiryDetails.Add(new InquiryDetail
        {
            Id = 1,
            InquiryHeaderId = 1,
            ProductId = 3
        });
        await data.SaveChangesAsync();
        
        var result = await inquiryDRepository.GetAllAsync();
        
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(1,data.InquiryDetails.Count());
        Assert.IsType<List<InquiryDetail>>(result);
    }
    
    [Fact]
    public void AddRangeTestMethod()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        var inquiryDRepository = new InquiryDetailsRepository(data);
        
        //Act
        var result = inquiryDRepository.AddRangeAsync(new List<InquiryDetail>()
        {
            new InquiryDetail
            {
                Id = 1,
                InquiryHeaderId = 1,
                ProductId = 2,
            },
            new InquiryDetail
            {
                Id = 2,
                InquiryHeaderId = 1,
                ProductId = 4
            }
        });
        
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(2,data.InquiryDetails.Count());
    }
    [Fact]
    public async void AddRangeTestMethodWithId()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        var inquiryDRepository = new InquiryDetailsRepository(data);
        
        //Act
        var result =  inquiryDRepository.AddRangeAsync(new List<InquiryDetail>()
        {
            new InquiryDetail
            {
                Id = 1,
                InquiryHeaderId = 1,
                ProductId = 2,
            },
            new InquiryDetail
            {
                Id = 2,
                InquiryHeaderId = 1,
                ProductId = 4
            },
            new InquiryDetail
            {
                Id = 3,
                InquiryHeaderId = 2,
                ProductId = 2
            }
        });

        var result2 =await inquiryDRepository.GetAllAsync(1);
        
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(3,data.InquiryDetails.Count());
        Assert.NotNull(result2);
        Assert.IsType<List<InquiryDetail>>(result2);
    }
    
    [Fact]
    public async void RemoveRangeTestMethodWithId()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        var inquiryDRepository = new InquiryDetailsRepository(data);
        
        //Act
        await inquiryDRepository.AddRangeAsync(new List<InquiryDetail>()
        {
            new InquiryDetail
            {
                Id = 1,
                InquiryHeaderId = 1,
                ProductId = 2,
            },
            new InquiryDetail
            {
                Id = 2,
                InquiryHeaderId = 1,
                ProductId = 4
            },
            new InquiryDetail
            {
                Id = 3,
                InquiryHeaderId = 2,
                ProductId = 2
            }
        });
        var items = new List<InquiryDetail>
        {
            new InquiryDetail
            {
                Id = 2,
                InquiryHeaderId = 1,
                ProductId = 4
            },
            new InquiryDetail
            {
                Id = 3,
                InquiryHeaderId = 2,
                ProductId = 2
            }
        };

        var result2 = inquiryDRepository.RemoveRangeAsync(items);
        await data.SaveChangesAsync();
        
        
        //Assert
        Assert.NotNull(result2);
    }
}