using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.EntryDbModels.Inquiry;

public class InquiryHeader
{
    [Key]
    public int Id { get; set; }

    [Required] 
    public string ApplicationUserId { get; set; } = null!;

    [ForeignKey(nameof(ApplicationUserId))]
    public ApplicationUser? ApplicationUser { get; set; }

    public DateTime InquiryDate { get; set; }

    [Required] 
    public string PhoneNumber { get; set; } = null!;

    [Required] 
    public string? FullName { get; set; } = null!;

    [Required] 
    public string Email { get; set; } = null!;
}