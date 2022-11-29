using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _repo;

        public CategoryController(ICategoryRepository repo)
        {
            _repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetAllAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}
