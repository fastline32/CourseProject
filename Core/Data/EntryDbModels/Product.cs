using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.EntryDbModels;

public class Product
{
    public Product()
    {
        TempQuantity = 1;
    }
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string? Description { get; set; }
    [Range(1,1500)]
    public double Price { get; set; }
    
    public string? Image { get; set; }
    public bool IsDeleted { get; set; }

    [Display(Name = ("Category Type"))]
    public int CategoryId { get; set; }
    [ForeignKey((nameof(CategoryId)))]
    public virtual Category? Category { get; set; }

    public int TypeId { get; set; }
    
    [ForeignKey(nameof(TypeId))]
    public virtual Type? Type { get; set; }

    [NotMapped]
    [Range(1, 1000, ErrorMessage = "Quantity must be greater than 0.")]
    public int TempQuantity { get; set; }

    public int? Quantity { get; set; }
}