﻿using System.Net;
using System.Net.Mail;
using System.Text;

namespace Bulky.Utility
{
    public class EmailSender : IEmailSender
    {
        // SendGridSecret = _config.GetValue<string>("SendGrid:SecretKey")!;

        public void SendEmailAsync(string email, string subject, string htmlMessage)
        {

            var from = "emailertodotnet@gmail.com";
            var to = email;

            MailMessage mailMessage = new MailMessage(from, to);

            mailMessage.Body = htmlMessage;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.UTF8;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            NetworkCredential networkCredential = new NetworkCredential(from, "nsxk mvbh cfuv eblc");
            smtpClient.Credentials = networkCredential;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;

            try
            {
                smtpClient.SendAsync(mailMessage, null);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
