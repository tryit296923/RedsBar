using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Alcoholic.Controllers.API
{
    [Route("api/Booking/[action]")]
    [ApiController]
    public class BookingApiController : Controller
    {
        private readonly db_a8de26_projectContext _db;
        public BookingApiController(db_a8de26_projectContext projectContext)
        {
            _db = projectContext;
        }

        [HttpGet]
        public object GetFrontBookings()
        {
            DateTime mindate = DateTime.Now;
            var bookingArr1 = (from x in _db.Reserves
                               where x.ReserveDate > mindate
                               group x by x.ReserveDate into g
                               orderby g.Key
                               select new { Date = g.Key, Total = g.Sum(x => x.Number) });

            return bookingArr1;
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

        [HttpGet]
        public IEnumerable<DataCenterBookingModel> GetTodayBookings()
        {
            DateTime today = DateTime.Now;
            var bookingArr = from x in _db.Reserves
                             where x.ReserveDate == today
                             orderby x.ReserveSet
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
