using System.Security.Claims;
using Core.Data.EntryDbModels;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<ApplicationUser> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByNameAsync(id);
        return user;
    }
}