using Infrastructure.DTOs;
using AutoMapper;
using Core.Data.EntryDbModels;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Roles = WebConstants.AdminRole +","+WebConstants.EditorRole )]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View(_repo.GetAll(x => x.IsDeleted == false));
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
                TempData[WebConstants.Error] = "Error while creating category";
                return View(model);
            }

            var foundItem = await _repo.FirstOrDefault(x => x.Name == model.Name);
            if (foundItem!=null)
            {
                ModelState.AddModelError("","This category already exist");
                return View(model);
            }

            var item = _mapper.Map<EntryCategoryViewModel, Category>(model);

            await _repo.Add(item);
            await _repo.Save();
            TempData[WebConstants.Success] = "Category created successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var model = _repo.Find(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Category,CategoryViewModel>(model));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var item = _repo.Find(id);

            if (item == null)
            {
                return NotFound();
            }
            
            item = _mapper.Map<CategoryViewModel, Category>(model);
            item.Id = id;
            await _repo.Update(item);
            await _repo.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return View(_mapper.Map<Category, CategoryViewModel>((_repo.Find(id))!));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var item = _repo.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            item.IsDeleted = true;
            await _repo.Update(item);
            await _repo.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
