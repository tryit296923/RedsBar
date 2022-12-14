using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Models.ViewModels;
using Alcoholic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Alcoholic.Controllers
{
    public class BookingController : Controller
    {
        private readonly MailService mail;
        private readonly db_a8de26_projectContext _db;

        public BookingController(MailService mailService,db_a8de26_projectContext projectContext)
        {
            mail = mailService;
            _db = projectContext;
        }
        public IActionResult Booking()
        {
            return View();
        }
        public IActionResult Check(BookingCheckModel bookingData)
        {
            var model = new BookingCheckModel() {
                ReserveDate = bookingData.ReserveDate,
                Number = bookingData.Number,
            };
            return View(model);
        }
        public async Task<IActionResult> Success([FromForm] BookingModel bookingData)
        {
            Reserves reserves = new Reserves() {
                ReserveDate = bookingData.ReserveDate,
                ReserveName = bookingData.ReserveName,
                Phone = bookingData.Phone,
                Email = bookingData.Email,
                Number = bookingData.Number,
                ReserveSet = DateTime.Now,
                Status = 1,
            };
            var task = Razor.Templating.Core.RazorTemplateEngine.RenderAsync<BookingModel>
                ("Views/EmailTemplate/BookingTemplate.cshtml", bookingData);
            _db.Add(reserves);
            _db.SaveChanges();
            var result = await task;
            mail.SendMail(bookingData.Email, result, "RedsBar感謝您的訂位");

            return View(bookingData);
        }
    }
}
