using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Core.Data.EntryDbModels;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string FullName { get; set; } = null!;
    public string? DisplayName { get; set; }
    
    public bool IsDeleted { get; set; } = false;

    public int? AddressId { get; set; }

    [ForeignKey(nameof(AddressId))]
    public Address? Address { get; set; }
}