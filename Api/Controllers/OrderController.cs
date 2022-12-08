using Api.Helpers.BrainTree;
using Braintree;
using Core.Interfaces;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Api.Controllers;

public class OrderController : Controller
{
    private readonly IOrderHeaderRepository _orderHRepository;
    private readonly IOrderDetailsRepository _orderDRepository;
    private readonly IBrainTreeGate _braintreeGate;

    [BindProperty]
    public OrderViewModel viewModel { get; set; }
    
    public OrderController(IOrderHeaderRepository orderHRepository,
        IOrderDetailsRepository orderDRepository,
        IBrainTreeGate braintreeGate)
    {
        _orderHRepository = orderHRepository;
        _orderDRepository = orderDRepository;
        _braintreeGate = braintreeGate;
    }
    // GET
    public IActionResult Index(string? searchName ,string? searchEmail, string? searchPhone, string? Status)
    {
        var item = new OrderListViewModel()
        {
            OrderHeaders = _orderHRepository.GetAll(),
            StatusListItems = WebConstants.listStatus.ToList().Select(x => new SelectListItem
            {
                Text = x,
                Value = x
            })
        };

        if (!string.IsNullOrEmpty(searchName))
        {
            item.OrderHeaders = item.OrderHeaders.Where(x => x.FullName.ToLower().Contains(searchName.ToLower()));
        }
        if (!string.IsNullOrEmpty(searchEmail))
        {
            item.OrderHeaders = item.OrderHeaders.Where(x => x.Email!.ToLower().Contains(searchName!.ToLower()));
        }
        if (!string.IsNullOrEmpty(searchPhone))
        {
            item.OrderHeaders = item.OrderHeaders.Where(x => x.PhoneNumber.ToLower().Contains(searchName!.ToLower()));
        }
        if (!string.IsNullOrEmpty(Status) && Status != "-- Order Status --")
        {
            item.OrderHeaders = item.OrderHeaders.Where(x => x.OrderStatus!.ToLower().Contains(searchName!.ToLower()));
        }
        return View(item);
    }

    public async Task<IActionResult> Details(int id)
    {
        viewModel = new OrderViewModel()
        {
            OrderHeader = await _orderHRepository.FirstOrDefault(x => x.Id == id),
            OrderDetails = _orderDRepository.GetAll(x => x.OrderHeaderId == id,includeProperties:"Product")
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> StartProcessing()
    {
        var orderHeader = await _orderHRepository.FirstOrDefault(x => x.Id == viewModel.OrderHeader.Id);
        orderHeader.OrderStatus = WebConstants.StatusInProcess;
        _orderHRepository.Update(orderHeader);
        await _orderHRepository.Save();
        TempData[WebConstants.Success] = "Order Is in Process";
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    public async Task<IActionResult> ShipOrder()
    {
        var orderHeader = await _orderHRepository.FirstOrDefault(x => x.Id == viewModel.OrderHeader.Id);
        orderHeader.OrderStatus = WebConstants.StatusShipped;
        orderHeader.ShippingDate = DateTime.UtcNow;
        _orderHRepository.Update(orderHeader);
        await _orderHRepository.Save();
        TempData[WebConstants.Success] = "Order Shiped Successfully";
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    public async Task<IActionResult> CancelOrder()
    {
        var orderHeader = await _orderHRepository.FirstOrDefault(x => x.Id == viewModel.OrderHeader.Id);
        var gateway = _braintreeGate.GetGateway();
        Transaction transaction =await gateway.Transaction.FindAsync(orderHeader.TransactionId);

        if (transaction.Status == TransactionStatus.AUTHORIZED || transaction.Status == TransactionStatus.SUBMITTED_FOR_SETTLEMENT)
        {
            Result<Transaction> resultVoid = await gateway.Transaction.VoidAsync(orderHeader.TransactionId);
        }
        else
        {
            Result<Transaction> resultRefund = await gateway.Transaction.RefundAsync(orderHeader.TransactionId);
        }

        orderHeader.OrderStatus = WebConstants.StatusRefunded;
        _orderHRepository.Update(orderHeader);
        await _orderHRepository.Save();
        TempData[WebConstants.Success] = "Order Canceled Successfully";
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateOrderDetails()
    {
        var orderHeader = await _orderHRepository.FirstOrDefault(x => x.Id == viewModel.OrderHeader.Id);
        orderHeader.FullName = viewModel.OrderHeader.FullName;
        orderHeader.City = viewModel.OrderHeader.City;
        orderHeader.PhoneNumber = viewModel.OrderHeader.PhoneNumber;
        orderHeader.StreetAddress = viewModel.OrderHeader.StreetAddress;
        orderHeader.ZipCode = viewModel.OrderHeader.ZipCode;
        orderHeader.Email = viewModel.OrderHeader.Email;
        _orderHRepository.Update(orderHeader);
        await _orderHRepository.Save();
        TempData[WebConstants.Success] = "Order Details Updated Successfully";
        return RedirectToAction("Details","Order",new {id = orderHeader.Id});
    }
}