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
        public IActionResult GetFrontBookings()
        {
            DateTime mindate = DateTime.Now;
            var bookingArr = (from x in _db.Reserves
                               where x.ReserveDate > mindate
                               group x by x.ReserveDate into g
                               orderby g.Key
                               select new { Date = g.Key, Total = g.Sum(x => x.Number) });

            return Ok(bookingArr);
        }

        // 曾被訂位的日期
        [HttpGet]
        public IActionResult GetAllBookingDate()
        {
            var allBookingDate = (from x in _db.Reserves
                                  group x by x.ReserveDate into g
                                  orderby g.Key
                                  select new { Date = g.Key }).ToList();
            return Ok(allBookingDate);
        }

        // 從後台查詢指定日期之訂位
        [HttpPost]
        public IActionResult SearchBooking([FromBody] SearchBookingModel booking)
        {
            var bookingInfo = (from x in _db.Reserves
                               where x.ReserveDate == booking.Date
                               select x).ToList();
            return Ok(bookingInfo);
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
        public IActionResult EditBooking([FromBody] EditBookingModel bookindData)
        {
            var bookingInfo = (from x in _db.Reserves
                              where x.ReserveId == bookindData.Id
                              select x).FirstOrDefault();
            bookingInfo.Number = bookindData.Number;
            bookingInfo.ReserveDate = bookindData.Date;
            _db.Update(bookingInfo);
            _db.SaveChanges();
            return Ok(true);
        }

        //從後台刪除訂位
        [HttpPost]
        public IActionResult DeleteBooking(int id)
        {
            var bookingInfo = (from x in _db.Reserves
                               where x.ReserveId == id
                               select x).FirstOrDefault();
            //bookingInfo.狀態 = 0;
            //_db.Update(bookingInfo);
            //_db.SaveChanges();
            return Ok(true);
        }
    }
}
