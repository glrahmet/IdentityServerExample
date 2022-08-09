using System.Net.Mail;

namespace IdentityServerExample.Helper
{
    public class EmailConfirmation
    {

        public static void EmailConfirmationMethod(string link, string email)
        {
            MailMessage mail = new MailMessage();

            SmtpClient client = new SmtpClient();

            mail.From = new MailAddress(email);
            mail.To.Add(email);

            mail.Subject = $"email doğrulama";
            mail.Body = "<h2>Email adresinizi doğrulamak için lütfen aşağıdaki linke tıklayınız</h2><hr>";
            mail.Body += $"<a href='{link}'>Email doğrulama linki</a>";
            mail.IsBodyHtml = true;
            client.Port = 595;
            client.Credentials = new System.Net.NetworkCredential("email adres", "şifre");

            client.Send(mail);

        }
    }
}
