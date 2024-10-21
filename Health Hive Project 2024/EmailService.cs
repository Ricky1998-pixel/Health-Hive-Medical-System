using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Health_Hive_Project_2024
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendEmailAsync(string toEmail, string name, string surname, string username, string password);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            string fromAddress = _configuration["EmailSettings:FromAddress"];
            string smtpHost = _configuration["EmailSettings:SmtpHost"];
            int smtpPort = _configuration.GetValue<int>("EmailSettings:SmtpPort");
            string smtpUsername = _configuration["EmailSettings:SmtpUsername"];
            string smtpPassword = _configuration["EmailSettings:SmtpPassword"];

            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromAddress),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };
                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
            }
        }
        //old code
        public async Task SendEmailAsync(string toEmail, string name, string surname, string username, string password)
        {
            string subject = "GROUP 16";
            string body = $"Dear {name} {surname},\n\nHere are your login credentials for the Health Hive System:\n\nUsername: {username}\nPassword: {password}\n\nPlease keep these credentials secure and do not share them with anyone.\n\nBest regards,\nThe Health Hive Team";

            await SendEmailAsync(toEmail, subject, body);
        }



    }
}
