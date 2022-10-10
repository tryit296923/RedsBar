using Alcoholic.Models.ViewModels;
using Alcoholic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Alcoholic.Controllers
{
    public class BookingController : Controller
    {
        private readonly MailService mailService;

        public BookingController(MailService mailService)
        {
            this.mailService = mailService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Check(string datepicker,int people)
        {
            ViewBag.Datepicker = datepicker;
            ViewBag.People = people;
            return View();
        }
        public async Task<IActionResult> Success(BookingSuccessViewModels data)
        {
            var result = await Razor.Templating.Core.RazorTemplateEngine.RenderAsync<BookingSuccessViewModels>("Views/EmailTemplate/BookingTemplate.cshtml",data);
            mailService.SendMail(data.Email,result,"感謝您的訂位");

            return View(data);
        }

    }
}
