using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs;

public class EntryTypeModel
{
    [Required]
    public string Name { get; set; } = null!;
}