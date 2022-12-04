using Core;
using Core.Data.EntryDbModels;
using Core.Interfaces;

namespace Infrastructure.Repositories;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _db;

    public CartService(ApplicationDbContext db)
    {
        _db = db;
    }

    public IEnumerable<Product> GetAllProductsAsync(List<int> listItems)
    {
        var result = _db.Products.Where(x => x.IsDeleted == false && listItems.Contains(x.Id)).AsEnumerable();
        return result;
    }
}