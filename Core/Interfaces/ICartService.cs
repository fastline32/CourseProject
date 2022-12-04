using Core.Data.EntryDbModels;

namespace Core.Interfaces;

public interface ICartService
{
    IEnumerable<Product> GetAllProductsAsync(List<int> listItems);
}