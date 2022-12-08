using Core.Data.EntryDbModels.Order;

namespace Infrastructure.DTOs;

public class OrderViewModel
{
    public OrderHeader OrderHeader { get; set; }
    public IEnumerable<OrderDetails> OrderDetails { get; set; }
}