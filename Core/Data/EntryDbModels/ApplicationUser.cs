using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Core.Data.EntryDbModels;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public string? DisplayName { get; set; }
    
    public bool IsDeleted { get; set; } = false;

    public int AddressId { get; set; }

    [ForeignKey(nameof(AddressId))]
    public Address Address { get; set; }
}