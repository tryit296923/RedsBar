using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;

namespace Alcoholic.Controllers.API
{
    [Route("api/Order/[action]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly db_a8de26_projectContext _db;

        public OrderApiController(db_a8de26_projectContext projectContext)
        {
            this._db = projectContext;
        }
        [HttpPost]
        public IActionResult Confirm([FromBody] OrderViewModel orderdata)
        {
            string sMemberID = HttpContext.Session.GetString("MemberID");
            string sDesk = HttpContext.Session.GetString("Desk");
            string sNumber = HttpContext.Session.GetString("Number");

            var now = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            var orderId = DateTime.Now.ToString("yyyyMMddHHmm") + sDesk;

            try
            {
                //分別存入Order, OrderDetail
                var order = new Order
                {
                    MemberId = Guid.Parse(sMemberID),
                    OrderId = orderId,
                    Number = int.Parse(sNumber),
                    DeskNum = sDesk,
                    OrderTime = Convert.ToDateTime(now),
                    Feedback = null
                };
                _db.Add(order);

                foreach (var item in orderdata.ItemList)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = orderId,
                        ProductId = item.Id,
                        Quantity = item.Qty,
                        UnitPrice = item.UnitPrice ?? 0,
                        Discount = item.DiscountAmount ?? 0,
                        Total = Convert.ToInt32(item.Qty * item.UnitPrice * item.DiscountAmount),
                    };
                    _db.Add(orderDetail);
                }
                _db.SaveChanges();

                return new JsonResult(new { Status = 1, Message = "Save Success", OrderId = orderId });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Status = 2, Message = "Save Fail" });
            }

        }

        // 離席
        [HttpPut]
        public IActionResult Dismiss(int Desk)
        {
            DeskInfo? deskInfo = (from desk in _db.DeskInfo
                                  where desk.Desk == Desk
                                  select desk).FirstOrDefault();
            deskInfo.Occupied = 0;
            deskInfo.EndTime = DateTime.Now.ToString("yyyyMMddHHmm");
            _db.Entry(deskInfo).State = EntityState.Modified;
            _db.SaveChanges();
            return Ok();
        }

    }
}
