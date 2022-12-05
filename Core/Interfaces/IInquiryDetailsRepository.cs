using Core.Data.EntryDbModels.Inquiry;

namespace Core.Interfaces;

public interface IInquiryDetailsRepository
{
    Task AddRangeAsync(List<InquiryDetail> items);
    Task<IEnumerable<InquiryDetail>> GetAllAsync();
    Task<IEnumerable<InquiryDetail>> GetAllAsync(int id);
}