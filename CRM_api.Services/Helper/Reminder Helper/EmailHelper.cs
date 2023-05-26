using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;


namespace CRM_api.Services.Helper.Reminder_Helper
{
    public class EmailHelper
    {
        public static void SendMailAsync(IConfiguration configuration, string recieverEmail, string subject, BodyBuilder body)
        {
            var email = new MimeMessage();
            var userName = configuration["MailSettings:UserName"];
            email.From.Add(MailboxAddress.Parse(userName));
            email.To.Add(MailboxAddress.Parse(recieverEmail));

            email.Subject = subject;
            email.Body = body.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(configuration["MailSettings:Host"], Convert.ToInt32(configuration["MailSettings:Port"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(configuration["MailSettings:UserName"], configuration["MailSettings:Password"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}

