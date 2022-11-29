using Core;
using Core.Data.EntryDbModels;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _db;

    public CategoryRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _db.Categories.ToListAsync();
    }

    public bool FindByNameAsync(string name)
    {
        return _db.Categories.Any(x => x.Name == name);
    }

    public async Task AddItemToDbAsync(Category item)
    {
        await _db.AddAsync(item);
        await _db.SaveChangesAsync();
    }

    public async Task<Category?> GetByIdAsync(int? id)
    {
        return await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);

    }
}