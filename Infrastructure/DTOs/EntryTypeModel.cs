using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs;

public class EntryTypeModel 
{
    [Required]
    [StringLength(50,MinimumLength = 5)]
    public string Name { get; set; } = null!;

    public bool IsDeleted
    {
        get;
        set;
    }
}