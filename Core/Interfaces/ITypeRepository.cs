using Type = Core.Data.EntryDbModels.Type;

namespace Core.Interfaces;

public interface ITypeRepository
{
    Task<IEnumerable<Type>> GetAllAsync();
    bool FindByNameAsync(string name);
    Task AddItemToDbAsync(Type item);
}