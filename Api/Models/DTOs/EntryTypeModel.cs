using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs;

public class EntryTypeModel
{
    [Required]
    public string Name { get; set; } = null!;
}