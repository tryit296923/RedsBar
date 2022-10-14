using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Models.ViewModels;
using Alcoholic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alcoholic.Controllers
{
    public class BookingController : Controller
    {
        private readonly MailService mailService;
        private readonly db_a8de26_projectContext _db;

        public BookingController(MailService mailService,db_a8de26_projectContext projectContext)
        {
            this.mailService = mailService;
            this._db = projectContext;
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
            var task = Razor.Templating.Core.RazorTemplateEngine.RenderAsync<Reserves>("Views/EmailTemplate/BookingTemplate.cshtml", bookingData);
            bookingData.ReserveSet = DateTime.Now;
            _db.Add(bookingData);
            _db.SaveChanges();
            var result = await task;
            mailService.SendMail(bookingData.Email, result, "感謝您的訂位");

            return View(bookingData);
        }

        [HttpGet]
        public Dictionary<DateTime,int> GetAllBookings()
        {
            DateTime mindate = DateTime.Now.AddDays(1);
            var bookingArr2 = _db.Reserves.Where(x => x.ReserveDate > mindate).GroupBy(x => x.ReserveDate)
                                            .ToDictionary(k => k.Key, v => v.Sum(n => n.Number));
            return bookingArr2;
        }
    }
}
