using Core.Data.EntryDbModels.Order;

namespace Core.Interfaces;

public interface IOrderHeaderRepository : IRepository<OrderHeader>
{
    void Update(OrderHeader item);
}