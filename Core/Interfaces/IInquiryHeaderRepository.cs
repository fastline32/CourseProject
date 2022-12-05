using Core.Data.EntryDbModels.Inquiry;

namespace Core.Interfaces;

public interface IInquiryHeaderRepository
{
    Task AddAsync(InquiryHeader inquiryHeader);
    Task<IEnumerable<InquiryHeader>> GetAll();
    Task<InquiryHeader> GetById(int id);
}