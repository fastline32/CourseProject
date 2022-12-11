using Core.Data.EntryDbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize(Roles = WebConstants.AdminRole)]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    // GET
    public IActionResult GetAllUsers()
    {
        var items = _userManager.Users;
        return View(items);
    }
    
    public IActionResult User(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }
        var item = _userManager.Users.FirstOrDefault(x => x.Id == id);
        if (item == null)
        {
            return NotFound();
        }
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeactivateProfile(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        user.IsDeleted = true;
        user.DeactivationDate = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
        return RedirectToAction(nameof(GetAllUsers));
    }

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeToEditorRole(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        var roles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, roles);
        await _userManager.AddToRoleAsync(user, WebConstants.EditorRole);
        return RedirectToAction(nameof(GetAllUsers));
    }
    

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReactivateUser(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        user.IsDeleted = false;
        await _userManager.UpdateAsync(user);
        return RedirectToAction(nameof(GetAllUsers));
    }
}