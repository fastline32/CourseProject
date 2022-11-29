using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs;

public class EntryCategoryViewModel
{
    [Required] 
    public string Name { get; set; } = null!;

    public int DisplayOrder { get; set; }
}