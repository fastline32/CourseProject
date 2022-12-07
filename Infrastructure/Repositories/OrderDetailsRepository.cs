using Core;
using Core.Data.EntryDbModels.Order;
using Core.Interfaces;

namespace Infrastructure.Repositories;

public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
{
    private readonly ApplicationDbContext _db;

    public OrderDetailsRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(OrderDetails item)
    {
        _db.OrderDetails.Update(item);
    }

    public async Task AddRangeAsync(IEnumerable<OrderDetails> item)
    {
        _db.OrderDetails.AddRange(item);
        await _db.SaveChangesAsync();
    }
}