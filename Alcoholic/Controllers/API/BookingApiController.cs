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
            var bookingArr = (from x in _db.Reserves
                               where x.ReserveDate > mindate
                               group x by x.ReserveDate into g
                               orderby g.Key
                               select new { Date = g.Key, Total = g.Sum(x => x.Number) });

            return bookingArr;
        }

        // 後台/bookings/Add
        [HttpPost]
        public void AddBooking([FromForm] BookingModel bookingData)
        {
            Reserves reserves = new Reserves()
            {
                ReserveDate = bookingData.ReserveDate,
                ReserveName = bookingData.ReserveName,
                Phone = bookingData.Phone,
                Email = bookingData.Email,
                Number = bookingData.Number,
                ReserveSet = DateTime.Now,
            };
            _db.Add(reserves);
            _db.SaveChanges();
        }
    }
}
