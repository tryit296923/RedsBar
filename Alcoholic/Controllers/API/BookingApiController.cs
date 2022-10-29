using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Models.ViewModels;
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

        // 統計30天內訂位人數
        [HttpGet]
        public IActionResult GetFrontBookings()
        {
            DateTime mindate = DateTime.Now;
            var bookingArr = (from x in _db.Reserves
                               where x.ReserveDate > mindate && x.Status == 1
                               group x by x.ReserveDate into g
                               orderby g.Key
                               select new { Date = g.Key, Total = g.Sum(x => x.Number) });

            return Ok(bookingArr);
        }

        // 當日訂位資訊
        [HttpGet]
        public IActionResult TodayBooking()
        {
            DateTime day = DateTime.Now;
            var todayBooking = (from x in _db.Reserves
                                where x.ReserveDate == day && x.Status != 0
                                select new
                                {
                                    Id = x.ReserveId,
                                    ReserveName = x.ReserveName,
                                    Number = x.Number,
                                    Phone = x.Phone,
                                    SetDate = x.ReserveSet,
                                    Status = x.Status,
                                }).ToList();
            return Ok(todayBooking);
        }

        // 所有曾被訂位的日期 for 後台查詢datepicker
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
                               orderby x.Status descending
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
                Status = 1,
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
        public IActionResult DeleteBooking([FromBody] DeleteBookingModel bookindData)
        {
            var bookingInfo = (from x in _db.Reserves
                               where x.ReserveId == bookindData.Id
                               select x).FirstOrDefault();
            bookingInfo.Status = 0;
            _db.Update(bookingInfo);
            _db.SaveChanges();
            return Ok(true);
        }
    }
}
