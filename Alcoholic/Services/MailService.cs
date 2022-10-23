using System.Linq.Expressions;
using System.Net.Mail;

namespace Alcoholic.Services
{
    public class MailService
    {
        public bool SendMail(string reciever, string msg, string title)
        {
            try
            {
                SmtpClient client = new("smtp.gmail.com", 587)
                {
                    Credentials = new System.Net.NetworkCredential("erictgm102@gmail.com", "syckjaoquolzjvrp"),
                    EnableSsl = true
                };
                MailAddress from = new("erictgm102@gmail.com","RedsBar");
                MailAddress to = new(reciever);
                MailMessage message = new(from, to)
                {
                    Body = msg,
                    IsBodyHtml = true,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    Subject = title,
                    SubjectEncoding = System.Text.Encoding.UTF8
                };
                client.Send(message);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
