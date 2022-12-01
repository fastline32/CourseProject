using Core;
using Core.Data.EntryDbModels;
using Core.Interfaces;
using Infrastructure.DTOs;
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
        return await _db.Categories.Where(x => x.IsDeleted == false).ToListAsync();
    }

    public bool FindByNameAsync(string name)
    {
        return _db.Categories.Where(x => x.IsDeleted==false).Any(x => x.Name == name);
    }

    public async Task AddItemToDbAsync(Category item)
    {
        await _db.AddAsync(item);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        _db.Categories.Update(category);
        await _db.SaveChangesAsync();
    }

    public async Task<Category?> GetByIdAsync(int? id)
    {
        var item = await _db.Categories.AsNoTracking().Where(x => x.IsDeleted==false).FirstOrDefaultAsync(x => x.Id == id);
        return item;
    }

    public async Task<Category> GetByNameAsync(string name)
    {
        var item =await _db.Categories.Where(x => x.IsDeleted==false).FirstAsync(x => x.Name == name);
        return item;
    }
}