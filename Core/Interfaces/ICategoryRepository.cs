using Core.Data.EntryDbModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task Update(Category category);
}