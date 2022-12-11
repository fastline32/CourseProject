using System.Net;
using System.Net.Mail;
using System.Text;
using Core.Data.Models;
using Core.Interfaces;
using Microsoft.Extensions.Options;

namespace Api.Helpers;

public class EmailService : IEmailService
{
    private readonly IOptions<SMTPConfigModel> _smtpConfig;

    public EmailService(IOptions<SMTPConfigModel> smtpConfig)
    {
        _smtpConfig = smtpConfig;
    }
    
    public async Task SendEmail(UserEmailOptions userEmailOptions)
    {
        var mail = new MailMessage
        {
            Subject = userEmailOptions.Subject,
            Body = userEmailOptions.Body,
            From = new MailAddress(_smtpConfig.Value.SenderAddress, _smtpConfig.Value.SenderDisplayName),
            IsBodyHtml = _smtpConfig.Value.IsBodyHtml
        };

        foreach (var email in userEmailOptions.ToEmails)
        {
            mail.To.Add(email);
        }

        NetworkCredential networkCredential =
            new NetworkCredential(_smtpConfig.Value.UserName, _smtpConfig.Value.Password);

        SmtpClient smtpClient = new SmtpClient
        {
            Host = _smtpConfig.Value.Host,
            Port = _smtpConfig.Value.Port,
            EnableSsl = _smtpConfig.Value.EnableSsh,
            UseDefaultCredentials = _smtpConfig.Value.UseDefaultCredentials,
            Credentials = networkCredential
        };
        mail.BodyEncoding = Encoding.Default;
        await smtpClient.SendMailAsync(mail);
    }

    private string UpdatePlaceHolders(string text,List<KeyValuePair<string,string>>keyValuePairs)
    {
        if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
        {
            foreach (var keyValuePair in keyValuePairs)
            {
                if (text.Contains(keyValuePair.Key))
                {
                    text = text.Replace(keyValuePair.Key, keyValuePair.Value);
                }
            }
        }

        return text;
    }
}