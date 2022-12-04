using Api.Extensions;
using Core.Interfaces;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
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

    public IActionResult Remove(int id)
    {
        var shoppingCartList = new List<ShoppingCart>();
        if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
            && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Any())
        {
            shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
        }

        shoppingCartList.Remove(shoppingCartList.FirstOrDefault(x => x.ProductId == id));
        HttpContext.Session.Set(WebConstants.SessionCart,shoppingCartList);
        return RedirectToAction(nameof(Index));
    }
}