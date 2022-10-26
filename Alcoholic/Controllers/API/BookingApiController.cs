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

        //統計每日訂位人數
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

        //送所有訂位資訊給後台
        [HttpGet]
        public IEnumerable<DataCenterBookingModel> GetAllBookings()
        {
            var bookingArr = from x in _db.Reserves
                             orderby x.ReserveDate
                             select new DataCenterBookingModel
                             {
                                 Id = x.ReserveId,
                                 Date = x.ReserveDate,
                                 Name = x.ReserveName,
                                 Phone = x.Phone,
                                 Email = x.Email,
                                 Number = x.Number,
                                 SetDate = x.ReserveSet
                             };

            return bookingArr;
        }

        // 從後台新增訂位
        [HttpPost]
        public IActionResult AddBooking([FromBody] BookingModel bookingData)
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
            return Ok(true);
        }

        //從後台修改訂位
        [HttpPost]
        public IActionResult EditBooking()
        {
            return Ok(true);
        }
    }
}
