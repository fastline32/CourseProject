using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.EntryDbModels.Order;

public class OrderHeader
{
    [Key]
    public int Id { get; set; }

    public string? CreatedByUserId { get; set; }
    
    [ForeignKey("CreatedByUserId")]
    public ApplicationUser? CreatedBy { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }
    [Required]
    public DateTime ShippingDate { get; set; }
    [Required]
    public double FinalOrderTotal { get; set; }
    public string? OrderStatus { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime PaymentDueDate { get; set; }
    public string? TransactionId { get; set; }

    [Required] 
    public string PhoneNumber { get; set; } = null!;
    [Required] 
    public string StreetAddress { get; set; } = null!;
    [Required] 
    public string City { get; set; } = null!;
    [Required] 
    public string ZipCode { get; set; } = null!;
    [Required] 
    public string FullName { get; set; } = null!;
    public string? Email { get; set; }
}