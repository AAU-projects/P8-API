using P8_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace P8_API.Services
{
    public class MailService : IMailService
    {
        private readonly IAppSettings _appSettings;

        public MailService(IAppSettings appsettings)
        {
            _appSettings = appsettings;
        }

        public bool SendMail(string toEmail, string pincode)
        {
            try
            {
                // Load mail client
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(_appSettings.EmailAddress, _appSettings.EmailPassword);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                MailMessage mail = new MailMessage();

                // Setup the email
                mail.From = new MailAddress("aaurouteplanner@gmail.com");
                mail.To.Add(new MailAddress(toEmail));
                mail.Subject = "RoutePlanner: Your pincode has arrived!";
                mail.Body = $"You have requested a pincode for routeplanner!\n Here is your pincode: {pincode}";

                // Send mail
                smtpClient.Send(mail);
            }
            catch (Exception e) 
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }
    }
}
