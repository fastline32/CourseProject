using Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Core.Interfaces;
using Infrastructure.DTOs;

namespace Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository,ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var homeViewModel = new HomeViewModel()
            {
                Categories = await _categoryRepository.GetAllAsync(),
                Products = await _productRepository.GetAllAsync()
            };
            return View(homeViewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var viewModel = new DetailsViewModel()
            {
                Product = await _productRepository.GetByIdAsync(id),
                ExistInCart = false
            };
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}