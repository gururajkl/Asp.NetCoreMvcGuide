using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Utility
{
    public interface IEmailSender
    {
        public void SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
