using Core.Data.EntryDbModels.Order;

namespace Core.Interfaces;

public interface IOrderDetailsRepository : IRepository<OrderDetails>
{
    void Update(OrderDetails item);
    Task AddRangeAsync(IEnumerable<OrderDetails> item);
}