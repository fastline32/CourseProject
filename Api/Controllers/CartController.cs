using System.Security.Claims;
using System.Text;
using Api.Extensions;
using Core.Data.EntryDbModels;
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
    private readonly IProductRepository _productRepository;

    [BindProperty] 
    private ProductUserViewModel ProductUserViewModel { get; set; } = null!;

    public CartController(ICartService cartService,
        IAccountService accountService, 
        IWebHostEnvironment webHostEnvironment,
        IEmailSender emailSender,
        IInquiryHeaderRepository inquiryHeaderRepository,
        IInquiryDetailsRepository inquiryDetailsRepository,
        IProductRepository productRepository)
    {
        _cartService = cartService;
        _accountService = accountService;
        _webHostEnvironment = webHostEnvironment;
        _emailSender = emailSender;
        _inquiryHeaderRepository = inquiryHeaderRepository;
        _inquiryDetailsRepository = inquiryDetailsRepository;
        _productRepository = productRepository;
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
        IList<Product> prodList = new List<Product>();
        foreach (var carObj in shoppingCartList)
        {
            Product prodTemp = productList.FirstOrDefault(x => x.Id == carObj.ProductId);
            prodTemp.TempQuantity = carObj.TempQuantity;
            prodList.Add(prodTemp);
        }
        return View(productList.ToList());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Index")]
    public IActionResult IndexPost(IEnumerable<Product> items)
    {
        List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
        foreach (var product in items)
        {
            shoppingCarts.Add(new ShoppingCart()
            {
                ProductId = product.Id,
                TempQuantity = product.TempQuantity
            });
        }
        HttpContext.Session.Set(WebConstants.SessionCart,shoppingCarts);
        return RedirectToAction(nameof(Summary));
    }

    public async Task<IActionResult> Summary()
    {
        ApplicationUser applicationUser;
        if (User.IsInRole(WebConstants.AdminRole) || User.IsInRole(WebConstants.EditorRole))
        {
            if (HttpContext.Session.Get<int>(WebConstants.SessionInquiryId) != 0)
            {
                //Assign header by getting id from session and export from DB
                InquiryHeader inquiryHeader = await _inquiryHeaderRepository.GetById(HttpContext.Session.Get<int>(WebConstants.SessionInquiryId));
                applicationUser = new ApplicationUser()
                {
                    Email = inquiryHeader.Email,
                    PhoneNumber = inquiryHeader.PhoneNumber,
                    FullName = inquiryHeader.FullName
                };
            }
            else
            {
                applicationUser = new ApplicationUser();
            }
        }
        else
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            applicationUser =await _accountService.GetUserByIdAsync(userId);
        }
        

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
            ApplicationUser = applicationUser,
        };

        foreach (var product in shoppingCartList)
        {
            var temp = await _productRepository.GetByIdAsync(product.ProductId);
            temp.TempQuantity = product.TempQuantity;
            ProductUserViewModel.Products.Add(temp);
        }

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateCart(IEnumerable<Product> items)
    {
        List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
        foreach (var product in items)
        {
            shoppingCarts.Add(new ShoppingCart()
            {
                ProductId = product.Id,
                TempQuantity = product.TempQuantity
            });
        }
        HttpContext.Session.Set(WebConstants.SessionCart,shoppingCarts);
        return RedirectToAction(nameof(Index));
    }
}