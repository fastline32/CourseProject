using Microsoft.AspNetCore.Identity;

namespace Core.Data.EntryDbModels;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public string? DisplayName { get; set; }
    public bool IsDeleted { get; set; } = false;
}