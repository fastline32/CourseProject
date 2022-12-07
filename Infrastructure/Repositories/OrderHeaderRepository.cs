using Core;
using Core.Data.EntryDbModels.Order;
using Core.Interfaces;

namespace Infrastructure.Repositories;

public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
{
    private readonly ApplicationDbContext _db;

    public OrderHeaderRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(OrderHeader item)
    {
        _db.OrderHeaders.Update(item);
    }
}