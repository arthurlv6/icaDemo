using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ica.website.infrastructure
{
    public static class EmailWithLink
    {
        public static int Send(string toEmail,string fromEmail,string subject, string content)
        {
            try
            {
                var smtp = new SmtpClient();
                var msg = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject
                };
                msg.To.Add(new MailAddress(toEmail));
                msg.Body = content;
                msg.IsBodyHtml = true;
                smtp.Send(msg);
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}