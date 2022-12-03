using System.ComponentModel.DataAnnotations;
using Core.Data.EntryDbModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Infrastructure.DTOs;

public class ProductViewModel
{
    public Product Product { get; set; }
    public IEnumerable<SelectListItem>? CategorySelectList { get; set; }
    
    public IEnumerable<SelectListItem>? TypeSelectedList { get; set; }
}