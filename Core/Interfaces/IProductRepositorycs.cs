using Core.Data.EntryDbModels;

namespace Core.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    bool FindByNameAsync(string name);
    Task AddItemToDbAsync(Product item);
    void Update(Product type);
    Task<Product> GetByIdAsync(int id);
    Task<Product> GetByNameAsync(string name);
}