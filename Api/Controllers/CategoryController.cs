using Api.Models.DTOs;
using AutoMapper;
using Core.Data.EntryDbModels;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EntryCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_repo.FindByNameAsync(model.Name))
            {
                ModelState.AddModelError("","This category already exist");
                return View(model);
            }

            var item = _mapper.Map<EntryCategoryViewModel, Category>(model);

            await _repo.AddItemToDbAsync(item);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var model = await _repo.GetByIdAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Category,CategoryViewModel>(model));
        }
    }
}
