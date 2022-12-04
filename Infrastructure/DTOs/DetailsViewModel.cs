using Core.Data.EntryDbModels;

namespace Infrastructure.DTOs;

public class DetailsViewModel
{
    public DetailsViewModel()
    {
        Product = new Product();
    }
    public Product Product{ get; set; }
    public bool ExistInCart { get; set; }
}