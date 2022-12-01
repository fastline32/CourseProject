using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.DTOs;

public class CategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int DisplayOrder { get; set; }
}