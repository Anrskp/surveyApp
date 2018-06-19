using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace MailMicroService
{
    public class Mail
    {
        public string To { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }

        public async Task<bool> send(string to, string body, string subject)
        {
            bool success = false;
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("emailmicroservicetest@gmail.com", "atT3vje7tYbi");

                string from = "emailmicroservicetest@gmail.com";
                MailMessage mail = new MailMessage(from, to, subject, body);
                mail.BodyEncoding = UTF8Encoding.UTF8;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mail);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                ex.ToString();
            }
            return success;


        }
    }
}
