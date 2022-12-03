using Core.Data.EntryDbModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    bool FindByNameAsync(string name);
    Task AddItemToDbAsync(Category item);
    Task<Category?> GetByIdAsync(int? id);
    Task UpdateAsync(Category category);
    Task<Category> GetByNameAsync(string name);
    IEnumerable<SelectListItem> GetSelectListAsync();
}