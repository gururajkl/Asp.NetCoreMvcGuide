using System.Net;
using System.Net.Mail;
using System.Text;

namespace EmailSender
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var from = "emailertodotnet@gmail.com";
            var to = "gjsetty@gmail.com";

            MailMessage mailMessage = new MailMessage(from, to);

            string mailBody = "<h1>Just to check</h1><p>Hey there</p>";
            mailMessage.Subject = "Checking";
            mailMessage.Body = mailBody;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.UTF8;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            NetworkCredential networkCredential = new NetworkCredential(from, "nsxk mvbh cfuv eblc");
            smtpClient.Credentials = networkCredential;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Mail send successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}