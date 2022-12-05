using Core.Data.EntryDbModels.Inquiry;

namespace Infrastructure.DTOs;

public class InquiryViewModel
{
    public InquiryHeader InquiryHeader { get; set; }
    public IEnumerable<InquiryDetail> InquiryDetails { get; set; }
}