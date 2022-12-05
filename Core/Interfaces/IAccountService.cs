using Core.Data.EntryDbModels;

namespace Core.Interfaces;

public interface IAccountService
{
    Task<ApplicationUser> GetUserByIdAsync(string id);
}