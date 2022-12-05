using AutoMapper;
using Core.Interfaces;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using Type = Core.Data.EntryDbModels.Type;

namespace Api.Controllers
{
    public class TypeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITypeRepository _repo;

        public TypeController(IMapper mapper, ITypeRepository repo)
        {
            _mapper = mapper;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EntryTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_repo.FindByNameAsync(model.Name))
            {
                ModelState.AddModelError("", "This category already exist");
                return View(model);
            }

            var item = _mapper.Map<EntryTypeModel, Type>(model);

            await _repo.AddItemToDbAsync(item);
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
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

            return View(_mapper.Map<Type,TypeViewModel>(model));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,TypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var item =await _repo.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            item = _mapper.Map<TypeViewModel, Type>(model);
            
            await _repo.UpdateAsync(item);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return View(_mapper.Map<Type, TypeViewModel>((await _repo.GetByIdAsync(id))!));
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
