using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return View();
        }

        [HttpGet]
        public IEnumerable<DataCenterBookingModel> GetAllBookings()
        {
            var bookingArr = from x in _db.Reserves
                             orderby x.ReserveDate
                             select new DataCenterBookingModel
                             {
                                 Date = x.ReserveDate,
                                 Name = x.ReserveName,
                                 Phone = x.Phone,
                                 Number = x.Number,
                                 SetDate = x.ReserveSet
                             };

            return bookingArr;
        }
    }
}
