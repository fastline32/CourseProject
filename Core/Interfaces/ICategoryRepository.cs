using Core.Data.EntryDbModels;

namespace Core.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    bool FindByNameAsync(string name);
    Task AddItemToDbAsync(Category item);
    Task<Category?> GetByIdAsync(int? id);
}