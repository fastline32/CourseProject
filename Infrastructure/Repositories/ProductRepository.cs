using Core;
using Core.Data.EntryDbModels;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _db;

    public ProductRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _db.Products
            .Include(x => x.Category)
            .Include(x => x.Type)
            .Where(x => x.IsDeleted == false).ToListAsync();
    }

    public bool FindByNameAsync(string name)
    {
        return _db.Products.Where(x => x.IsDeleted==false).Any(x => x.Name == name);
    }

    public async Task AddItemToDbAsync(Product item)
    {
        await _db.AddAsync(item);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product item)
    {
        _db.Products.Update(item);
        await _db.SaveChangesAsync();
    }

    public async Task<Product?> GetByIdAsync(int? id)
    {
        var item = await _db.Products.AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.Type)
            .Where(x => x.IsDeleted==false).FirstOrDefaultAsync(x => x.Id == id);
        return item;
    }

    public async Task<Product> GetByNameAsync(string name)
    {
        var item =await _db.Products.Where(x => x.IsDeleted==false).FirstAsync(x => x.Name == name);
        return item;
    }
}