using Core.Data.EntryDbModels.Inquiry;
using Infrastructure.Repositories;
using ShopProject.Test.Mocks;
using Xunit;

namespace ShopProject.Test.Services;

public class InquiryHeaderRepositoryTest
{
    [Fact]
    public async void GetAllTestMethod()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        
        var inquiryHRepository = new InquiryHeaderRepository(data);
        
        //Act
        await inquiryHRepository.AddAsync(new InquiryHeader
        {
            Id = 1,
            ApplicationUserId = "next",
            Email = "test@test.com",
            FullName = "Test",
            InquiryDate = DateTime.Now,
            PhoneNumber = "+333333333"
        });
        await inquiryHRepository.AddAsync(new InquiryHeader
        {
            Id = 2,
            ApplicationUserId = "next1",
            Email = "test@test3.com",
            FullName = "Test2",
            InquiryDate = DateTime.Now,
            PhoneNumber = "+333333333"
        });

        var result =await inquiryHRepository.GetAll();
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(2,data.InquiryHeaders.Count());
        Assert.IsType<List<InquiryHeader>>(result);
    }
    
    [Fact]
    public async void GetByIdTestMethod()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        
        var inquiryHRepository = new InquiryHeaderRepository(data);
        
        //Act
        await inquiryHRepository.AddAsync(new InquiryHeader
        {
            Id = 1,
            ApplicationUserId = "next",
            Email = "test@test.com",
            FullName = "Test",
            InquiryDate = DateTime.Now,
            PhoneNumber = "+333333333"
        });
        await inquiryHRepository.AddAsync(new InquiryHeader
        {
            Id = 2,
            ApplicationUserId = "next1",
            Email = "test@test3.com",
            FullName = "Test2",
            InquiryDate = DateTime.Now,
            PhoneNumber = "+333333333"
        });

        var result =await inquiryHRepository.GetById(1);
        var result2 = await inquiryHRepository.GetById(3);
        //Assert
        Assert.NotNull(result);
        Assert.IsType<InquiryHeader>(result);
        Assert.Null(result2);
    }
    
    [Fact]
    public async void RemoveTestMethod()
    {
        //Arrange
        var data = DatabaseMock.Instance;
        
        var inquiryHRepository = new InquiryHeaderRepository(data);
        
        //Act
        await inquiryHRepository.AddAsync(new InquiryHeader
        {
            Id = 1,
            ApplicationUserId = "next",
            Email = "test@test.com",
            FullName = "Test",
            InquiryDate = DateTime.Now,
            PhoneNumber = "+333333333"
        });
        await inquiryHRepository.AddAsync(new InquiryHeader
        {
            Id = 2,
            ApplicationUserId = "next1",
            Email = "test@test3.com",
            FullName = "Test2",
            InquiryDate = DateTime.Now,
            PhoneNumber = "+333333333"
        });

        List<InquiryHeader> items = new List<InquiryHeader>();
        var item =await inquiryHRepository.GetById(2);
        items.Add(item);
        //var result =  inquiryHRepository.Remove(item);
        //Assert
        Assert.Equal(1,data.InquiryHeaders.Count());
    }
}