using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs;

public class EntryCategoryViewModel
{
    [Required] 
    public string Name { get; set; } = null!;

    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; }
}