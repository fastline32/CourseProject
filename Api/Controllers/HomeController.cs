using Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Api.Extensions;
using Core.Interfaces;
using Infrastructure.DTOs;
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public HomeController(IProductRepository productRepository,ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var homeViewModel = new HomeViewModel()
            {
                Categories = _categoryRepository.GetAll(x => x.IsDeleted == false),
                Products = await _productRepository.GetAllAsync()
            };
            return View(homeViewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var shoppingList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Any())
            {
                shoppingList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
            }
            
            var viewModel = new DetailsViewModel()
            {
                Product = (await _productRepository.GetByIdAsync(id))!,
                ExistInCart = false
            };

            foreach (var shoppingCart in shoppingList)
            {
                if (shoppingCart.ProductId == id)
                {
                    viewModel.ExistInCart = true;
                }
            }
            return View(viewModel);
        }
        
        [HttpPost,ActionName("Details")]
        public IActionResult DetailsPost(int id,DetailsViewModel viewModel)
        {
            var shoppingList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Any())
            {
                shoppingList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
            }
            shoppingList.Add(new ShoppingCart{ProductId = id,TempQuantity = viewModel.Product.TempQuantity});
            HttpContext.Session.Set(WebConstants.SessionCart,shoppingList);
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult RemoveFromCart(int id)
        {
            var shoppingList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Any())
            {
                shoppingList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
            }

            var itemForRemove = shoppingList.SingleOrDefault(x => x.ProductId == id);
            if (itemForRemove != null)
            {
                shoppingList.Remove(itemForRemove);
            }
            
            HttpContext.Session.Set(WebConstants.SessionCart,shoppingList);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}