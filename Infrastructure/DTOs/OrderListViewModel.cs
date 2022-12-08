using Core.Data.EntryDbModels.Order;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Infrastructure.DTOs;

public class OrderListViewModel
{
    public IEnumerable<OrderHeader> OrderHeaders { get; set; }
    public IEnumerable<SelectListItem> StatusListItems { get; set; }
    public string Status { get; set; }
}