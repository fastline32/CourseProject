using Infrastructure.DTOs;
using AutoMapper;
using Core;
using Core.Data.EntryDbModels;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ITypeRepository _typeRepository;

        public ProductController(IProductRepository repo, IMapper mapper, ApplicationDbContext db,
            ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment, ITypeRepository typeRepository)
        {
            _repo = repo;
            _mapper = mapper;
            _db = db;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _typeRepository = typeRepository;
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
                CategorySelectList =_categoryRepository.GetSelectListAsync(),
                TypeSelectedList = _typeRepository.GetSelectListAsync()
            };
            
            if (id == null)
            {
                return View(viewModel);
            }
            else
            {
                viewModel.Product =await _db.Products.FindAsync(id);
                if (viewModel.Product == null)
                {
                    return NotFound();
                }

                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                
                if (model.Product.Id == 0)
                {
                    //Creating image path
                    string upload = webRootPath + WebConstants.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    
                    //Uploading image to server
                    using (var fileStream = new FileStream(Path.Combine(upload,fileName+extension),FileMode.Create))
                    {
                        await files[0].CopyToAsync(fileStream);
                    }

                    model.Product.Image = fileName + extension;
                    await _repo.AddItemToDbAsync(model.Product);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var item = _db.Products.AsNoTracking().FirstOrDefault(x => x.Id == model.Product.Id);
                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WebConstants.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFilePath = Path.Combine(upload, model.Product.Image);

                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                        
                        using (var fileStream = new FileStream(Path.Combine(upload,fileName+extension),FileMode.Create))
                        {
                            await files[0].CopyToAsync(fileStream);
                        }

                        model.Product.Image = fileName + extension;
                    }
                    else
                    {
                        model.Product.Image = item.Image;
                    }

                    _db.Products.Update(item);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            model.CategorySelectList = _categoryRepository.GetSelectListAsync();
            model.TypeSelectedList = _typeRepository.GetSelectListAsync();
            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await _repo.GetByIdAsync(id));
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
