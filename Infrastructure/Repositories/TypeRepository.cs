using Core;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Type = Core.Data.EntryDbModels.Type;

namespace Infrastructure.Repositories;

public class TypeRepository : ITypeRepository
{
    private readonly ApplicationDbContext _db;

    public TypeRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Type>> GetAllAsync()
    {
        return await _db.Types.ToListAsync();
    }

    public bool FindByNameAsync(string name)
    {
        return _db.Types.Any(x => x.Name == name);
    }

    public async Task AddItemToDbAsync(Type item)
    {
        await _db.AddAsync(item);
        await _db.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Type type)
    {
        _db.Types.Update(type);
        await _db.SaveChangesAsync();
    }

    public async Task<Type?> GetByIdAsync(int? id)
    {
        var item = await _db.Types.AsNoTracking().Where(x => x.IsDeleted==false).FirstOrDefaultAsync(x => x.Id == id);
        return item;
    }

    public async Task<Type> GetByNameAsync(string name)
    {
        var item =await _db.Types.Where(x => x.IsDeleted==false).FirstAsync(x => x.Name == name);
        return item;
    }
    public  IEnumerable<SelectListItem> GetSelectListAsync()
    {
        var items = _db.Types.Where(x => x.IsDeleted == false).Select(x => new SelectListItem
        {
            Text = x.Name,
            Value = x.Id.ToString()
        });
        return items;
    }
}