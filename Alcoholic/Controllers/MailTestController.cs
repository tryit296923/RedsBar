using Alcoholic.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace Alcoholic.Controllers
{
    public class MailTestController : Controller
    {
        private readonly MailService mailService;

        public MailTestController(MailService mailService)
        {
            this.mailService = mailService;
        }
        public void Index(string mailAddress, string msg, string title)
        {
            mailService.SendMail(mailAddress, msg, title);
        }
    }
}
