using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Models.ViewModels;
using Alcoholic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Alcoholic.Controllers
{
    public class BookingController : Controller
    {
        private readonly MailService mailService;
        private readonly db_a8de26_projectContext projectContext;

        public BookingController(MailService mailService,db_a8de26_projectContext projectContext)
        {
            this.mailService = mailService;
            this.projectContext = projectContext;
        }
        public IActionResult Booking()
        {
            return View();
        }
        public IActionResult Check(string ReserveDate, int Number)
        {
            ViewBag.ReserveDate = ReserveDate;
            ViewBag.Number = Number;
            return View();
        }
        public async Task<IActionResult> Success(Reserves bookingData)
        {
            bookingData.ReserveId = projectContext.Reserves.Count()+1;
            bookingData.ReserveSet = DateTime.Now;
            await projectContext.AddAsync(bookingData);
            await projectContext.SaveChangesAsync();
            var result = await Razor.Templating.Core.RazorTemplateEngine.RenderAsync<Reserves>("Views/EmailTemplate/BookingTemplate.cshtml", bookingData);
            mailService.SendMail(bookingData.Email,result,"感謝您的訂位");

            return View(bookingData);
        }

    }
}
