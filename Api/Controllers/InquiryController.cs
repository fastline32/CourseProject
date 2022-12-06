using Api.Extensions;
using Core.Interfaces;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class InquiryController : Controller
{
    private readonly IInquiryHeaderRepository _inquiryHeaderRepository;
    private readonly IInquiryDetailsRepository _inquiryDetailsRepository;
    
    [BindProperty]
    private InquiryViewModel ViewModel { get; set; }

    public InquiryController(IInquiryHeaderRepository inquiryHeaderRepository,
        IInquiryDetailsRepository inquiryDetailsRepository)
    {
        _inquiryHeaderRepository = inquiryHeaderRepository;
        _inquiryDetailsRepository = inquiryDetailsRepository;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public async Task<IActionResult> Details(int id)
    {
        ViewModel = new InquiryViewModel()
        {
            InquiryHeader = await _inquiryHeaderRepository.GetById(id),
            InquiryDetails = await _inquiryDetailsRepository.GetAllAsync(id)
        };
        return View(ViewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Details")]
    public async Task<IActionResult> DetailsPost(int id)
    {
        List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
        var item = await _inquiryDetailsRepository.GetAllAsync(id);
        foreach (var detail in item)
        {
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                ProductId = detail.ProductId
            };
            shoppingCarts.Add(shoppingCart);
        }
        HttpContext.Session.Clear();
        HttpContext.Session.Set(WebConstants.SessionCart,shoppingCarts);
        HttpContext.Session.Set(WebConstants.SessionInquiryId, id);
        return RedirectToAction("Index", "Cart");
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var inquiryHeader = await _inquiryHeaderRepository.GetById(id);
        var item = await _inquiryDetailsRepository.GetAllAsync(id);
        await _inquiryDetailsRepository.RemoveRangeAsync(item);
        await _inquiryHeaderRepository.Remove(inquiryHeader);
        
        return RedirectToAction("Index","Inquiry");
    }

    #region API CALLS

    [HttpGet]
    public async Task<IActionResult> GetInquiryList()
    {
        return Json(new {data = await _inquiryHeaderRepository.GetAll()});
    }
    #endregion
}