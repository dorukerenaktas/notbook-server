using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace NotBook.Service.Email
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _senderEmail;
        private readonly IHostingEnvironment _environment;

        public EmailService(IConfiguration configuration, IHostingEnvironment environment)
        {
            _smtpUsername = configuration.GetConnectionString("SMTPUserName");
            _smtpPassword = configuration.GetConnectionString("SMTPPassword");
            _senderEmail = configuration.GetConnectionString("SMTPUserName");
            _environment = environment;
        }

        public void SendVerificationEmail(string email, string verificationHash)
        {
            var body = CreateEmailBody("Assets/Template/VerificationEmailTemplate.html", $"https://notbook.net/verify/{verificationHash}");
            var subject = ExtractTitleFromEmailBody(body);
            Send(_senderEmail, email, subject, body);
        }

        public void SendForgotPasswordEmail(string email, string verificationHash)
        {
            var body = CreateEmailBody("Assets/Template/ForgotUserEmailTemplate.html", $"https://notbook.net/password/{verificationHash}");
            var subject = ExtractTitleFromEmailBody(body);
            Send(_senderEmail, email, subject, body);
        }

        private void Send(string senderEmail, string receiverEmail, string subject, string body)
        {
            using (var client = new SmtpClient("smtp.yandex.com")
            {
                // client.Port = 465; // Gives timeout error
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword)
            })
            {
                client.Send(new MailMessage
                {
                    Body = body,
                    Subject = subject,
                    From = new MailAddress(senderEmail),
                    To = {receiverEmail},
                    IsBodyHtml = true
                });
            }
        }

        private string CreateEmailBody(string path, string url)
        {
            string body;

            var root = _environment.ContentRootPath;
            using (var reader = File.OpenText(Path.Combine(root, path)))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{verification_url}", url);

            return body;
        }
        
        private static string ExtractTitleFromEmailBody(string body)
        {
            return body.Split(new[] {"<title>"}, StringSplitOptions.None)[1]
                .Split(new[] {"</title>"}, StringSplitOptions.None)[0]
                .Trim();
        }
    }
}