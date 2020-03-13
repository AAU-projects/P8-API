using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace P8_API.Services
{
    public class MailService : IMailService
    {
        public bool SendMail(string toEmail, string pincode)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential("aaurouteplanner@gmail.com", "p8projekt");
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                MailMessage mail = new MailMessage();

                //Setting From , To and CC
                mail.From = new MailAddress("aaurouteplanner@gmail.com");
                mail.To.Add(new MailAddress(toEmail));
                mail.Subject = "RoutePlanner: Your pincode has arrived!";
                mail.Body = $"You have requested a pincode for routeplanner!\n Here is your pincode: {pincode}";


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
