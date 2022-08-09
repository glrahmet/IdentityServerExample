using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IdentityServerExample.Helper
{
    public static class PasswordReset
    {
        public static void PasswordResetSendEmail(string link,string email)
        {
            MailMessage mail = new MailMessage();

            SmtpClient client = new SmtpClient();

            mail.From = new MailAddress(email);
            mail.To.Add("umutahmet_18@hotmail.com");

            mail.Subject = $"şifre sıfırlama";
            mail.Body = "<h2>şifrenizi yenilemek için lütfen aşağıdaki linke tıklayınız</h2><hr>";
            mail.Body += $"<a href='{link}'>Şifre yenileme linki</a>";
            mail.IsBodyHtml = true;
            client.Port = 595;
            client.Credentials = new System.Net.NetworkCredential("email adres", "şifre");

            client.Send(mail);
        }
    }
}
