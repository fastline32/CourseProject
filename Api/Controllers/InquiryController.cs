using Core.Interfaces;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class InquiryController : Controller
{
    private readonly IInquiryHeaderRepository _inquiryHeaderRepository;
    private readonly IInquiryDetailsRepository _inquiryDetailsRepository;

    public InquiryController(IInquiryHeaderRepository inquiryHeaderRepository,
        IInquiryDetailsRepository inquiryDetailsRepository)
    {
        _inquiryHeaderRepository = inquiryHeaderRepository;
        _inquiryDetailsRepository = inquiryDetailsRepository;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public async Task<IActionResult> Details(int id)
    {
        InquiryViewModel item = new InquiryViewModel()
        {
            InquiryHeader = await _inquiryHeaderRepository.GetById(id),
            InquiryDetails = await _inquiryDetailsRepository.GetAllAsync(id)
        };
        return View(item);
    }

    #region API CALLS

    [HttpGet]
    public async Task<IActionResult> GetInquiryList()
    {
        return Json(new {data = await _inquiryHeaderRepository.GetAll()});
    }
    #endregion
}