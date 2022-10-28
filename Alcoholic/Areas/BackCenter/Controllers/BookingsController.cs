using Alcoholic.Models.Entities;
using Alcoholic.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Alcoholic.Areas.BackCenter.Controllers
{
    [Area("BackCenter")]
    public class BookingsController : Controller
    {
        private readonly db_a8de26_projectContext _db;
        public BookingsController(db_a8de26_projectContext context)
        {
            _db=context;
        }
        public IActionResult Index()
        {
            DateTime day = DateTime.Now;
            var todayBooking = (from x in _db.Reserves
                                where x.ReserveDate == day && x.Status == 1
                                select new TodayBookingModel
                                {
                                    ReserveName = x.ReserveName,
                                    Number = x.Number,
                                    Phone = x.Phone,
                                    SetDate = x.ReserveSet
                                }).ToList();

            return View(todayBooking);
        }
    }
}
