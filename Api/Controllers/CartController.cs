using System.Security.Claims;
using System.Text;
using Api.Extensions;
using Core.Data.EntryDbModels.Inquiry;
using Core.Interfaces;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace Api.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IAccountService _accountService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IEmailSender _emailSender;
    private readonly IInquiryHeaderRepository _inquiryHeaderRepository;
    private readonly IInquiryDetailsRepository _inquiryDetailsRepository;

    [BindProperty] 
    private ProductUserViewModel ProductUserViewModel { get; set; } = null!;

    public CartController(ICartService cartService,
        IAccountService accountService, 
        IWebHostEnvironment webHostEnvironment,
        IEmailSender emailSender,
        IInquiryHeaderRepository inquiryHeaderRepository,
        IInquiryDetailsRepository inquiryDetailsRepository)
    {
        _cartService = cartService;
        _accountService = accountService;
        _webHostEnvironment = webHostEnvironment;
        _emailSender = emailSender;
        _inquiryHeaderRepository = inquiryHeaderRepository;
        _inquiryDetailsRepository = inquiryDetailsRepository;
    }

    // GET
    public IActionResult Index()
    {
        var shoppingCartList = new List<ShoppingCart>();
        if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
            && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Any())
        {
            shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
        }

        var products = shoppingCartList.Select(i => i.ProductId).ToList();
        var productList = _cartService.GetAllProductsAsync(products);
        return View(productList);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Index")]
    public IActionResult IndexPost()
    {

        return RedirectToAction(nameof(Summary));
    }

    public async Task<IActionResult> Summary()
    { var userId = User.FindFirstValue(ClaimTypes.Name);

        var shoppingCartList = new List<ShoppingCart>();
        if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
            && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Any())
        {
            shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
        }

        var products = shoppingCartList.Select(i => i.ProductId).ToList();
        var productList = _cartService.GetAllProductsAsync(products);

        ProductUserViewModel = new ProductUserViewModel()
        {
            ApplicationUser = await _accountService.GetUserByIdAsync(userId),
            Products = productList.ToList()
        };

        return View(ProductUserViewModel);
    }
    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Summary")]
    public async Task<IActionResult> SummaryPost(ProductUserViewModel productUserViewModel)
    {
        var claimsIdentity = (ClaimsIdentity) User.Identity!;
        var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        var templatePath = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString() + "templates"
                           + Path.DirectorySeparatorChar.ToString() + "Inquiry.html";

        var subject = "New Inquiry";
        string htmlBody;
        using (StreamReader sr = System.IO.File.OpenText(templatePath))
        {
            htmlBody =await sr.ReadToEndAsync();
        }

        StringBuilder sb = new StringBuilder();
        foreach (var product in productUserViewModel.Products)
        {
            sb.Append($" - Name: {product.Name} <span style='font-size:14px;'> (ID: {product.Id} </span><br/>");
        }

        string messageBody = string.Format(htmlBody,
            productUserViewModel.ApplicationUser.FullName,
            productUserViewModel.ApplicationUser.Email,
            productUserViewModel.ApplicationUser.PhoneNumber,
            sb);

        await _emailSender.SendEmailAsync(WebConstants.EmailAdmin, subject, messageBody);

        InquiryHeader inquiryHeader = new InquiryHeader()
        {
            ApplicationUserId = claims!.Value,
            FullName = productUserViewModel.ApplicationUser.FullName,
            Email = productUserViewModel.ApplicationUser.Email,
            PhoneNumber = productUserViewModel.ApplicationUser.PhoneNumber,
            InquiryDate = DateTime.UtcNow
        };
        
        await _inquiryHeaderRepository.AddAsync(inquiryHeader);
        
        var items = new List<InquiryDetail>();
        foreach (var product in productUserViewModel.Products)
        {
            
            InquiryDetail inquiryDetail = new InquiryDetail()
            {
                InquiryHeaderId = inquiryHeader.Id,
                ProductId = product.Id
            };
            items.Add(inquiryDetail);
        }
        await _inquiryDetailsRepository.AddRangeAsync(items);
        
        return RedirectToAction(nameof(InquiryConfirmation));
    }

    public IActionResult InquiryConfirmation()
    {
        HttpContext.Session.Clear();
        return View();
    }
    
    public IActionResult Remove(int id)
    {
        var shoppingCartList = new List<ShoppingCart>();
        if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
            && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Any())
        {
            shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
        }

        shoppingCartList.Remove(shoppingCartList.FirstOrDefault(x => x.ProductId == id)!);
        HttpContext.Session.Set(WebConstants.SessionCart,shoppingCartList);
        return RedirectToAction(nameof(Index));
    }
}