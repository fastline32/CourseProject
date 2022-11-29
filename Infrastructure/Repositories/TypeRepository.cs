using Core;
using Core.Interfaces;
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
}