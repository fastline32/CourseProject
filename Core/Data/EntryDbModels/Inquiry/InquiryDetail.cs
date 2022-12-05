using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.EntryDbModels.Inquiry;

public class InquiryDetail
{
    [Key]
    public int Id { get; set; }

    [Required] 
    public int InquiryHeaderId { get; init; }

    [ForeignKey(nameof(InquiryHeaderId))]
    public InquiryHeader? InquiryHeader { get; set; }

    [Required] 
    public int ProductId { get; init; }

    [ForeignKey(nameof(ProductId))] 
    public Product? Product { get; set; }
}