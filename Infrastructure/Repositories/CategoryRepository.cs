using Core;
using Core.Data.EntryDbModels;
using Core.Interfaces;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly ApplicationDbContext _db;

    public CategoryRepository(ApplicationDbContext db) 
        : base(db)
    {
        _db = db;
    }

    public async Task Update(Category category)
    {
        var item = await base.FirstOrDefault(x => x.Id == category.Id);
        if (item != null)
        {
            item.Name = category.Name;
            item.DisplayOrder = category.DisplayOrder;
            item.IsDeleted = category.IsDeleted;
        }
    }
}