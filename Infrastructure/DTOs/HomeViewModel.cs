using Core.Data.EntryDbModels;

namespace Infrastructure.DTOs;

public class HomeViewModel
{
    public IEnumerable<Product>? Products { get; set; }
    public IEnumerable<Category>? Categories { get; set; }
}