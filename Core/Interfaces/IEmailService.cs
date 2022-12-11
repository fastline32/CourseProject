using Core.Data.Models;

namespace Core.Interfaces;

public interface IEmailService
{
    Task SendEmail(UserEmailOptions userEmailOptions);
}