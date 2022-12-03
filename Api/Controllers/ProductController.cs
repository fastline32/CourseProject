using Infrastructure.DTOs;
using AutoMapper;
using Core;
using Core.Data.EntryDbModels;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository repo, IMapper mapper, ApplicationDbContext db,ICategoryRepository categoryRepository)
        {
            _repo = repo;
            _mapper = mapper;
            _db = db;
            _categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            ProductViewModel viewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategorySelectList = _categoryRepository.GetSelectListAsync()
            };
            
            if (id == null)
            {
                return View(viewModel);
            }
            else
            {
                viewModel.Product = _db.Products.Find(id);
                if (viewModel.Product == null)
                {
                    return NotFound();
                }

                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Upsert(EntryCategoryViewModel model)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return View(model);
        //     }
        //
        //     if (_repo.FindByNameAsync(model.Name))
        //     {
        //         ModelState.AddModelError("","This category already exist");
        //         return View(model);
        //     }
        //
        //     var item = _mapper.Map<EntryCategoryViewModel, Category>(model);
        //
        //     await _repo.AddItemToDbAsync(item);
        //     return RedirectToAction(nameof(Index));
        // }
        
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await _repo.GetByIdAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            item.IsDeleted = true;
            await _repo.UpdateAsync(item);
            return RedirectToAction(nameof(Index));
        }
    }
}
