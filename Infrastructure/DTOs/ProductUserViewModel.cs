using Core.Data.EntryDbModels;

namespace Infrastructure.DTOs;

public class ProductUserViewModel
{
    public ApplicationUser ApplicationUser { get; set; } = null!;
    public IList<Product> Products { get; set; } = new List<Product>();
}