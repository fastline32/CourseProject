using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs;

public class TypeViewModel
{
    public int Id
    {
        get;
        set;
    }
    
    [Required]
    public string Name { get; set; } = null!;

    public bool IsDeleted { get; set; }
}