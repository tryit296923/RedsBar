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

            var x = (from o in _db.Orders 
                     where o.MemberId == Guid.Parse(sMemberID) && o.Status == "N" 
                     select o.OrderId).FirstOrDefault();
            try
            {
                if(x == null)
                {
                    var now = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                    var orderId = DateTime.Now.ToString("yyyyMMddHHmm") + sDesk;
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
                            Sequence = 1,
                        };
                        _db.Add(orderDetail);
                    }
                }
                else
                {
                    var seq = (from s in _db.OrderDetails where s.OrderId == x select s.Sequence).LastOrDefault() + 1;
                    foreach (var item in orderdata.ItemList)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = x,
                            ProductId = item.Id,
                            Quantity = item.Qty,
                            UnitPrice = item.UnitPrice ?? 0,
                            Discount = item.DiscountAmount ?? 0,
                            Total = Convert.ToInt32(item.Qty * item.UnitPrice * item.DiscountAmount),
                            Sequence = seq,
                        };
                        _db.Add(orderDetail);
                    }
                }
                
                _db.SaveChanges();

                return new JsonResult(new { Status = 1, Message = "Save Success", OrderId = x });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Status = 2, Message = "Save Fail" });
            }

        }

        // 離席
        [HttpPut]
        public IActionResult Dismiss(string Desk)
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

        // 接收前端傳的金流串接所需資料(未付款)
        [HttpPost]
        public IActionResult OnlinePayment(string orderIdTotal, int totalPrice)
        {
            TradeInfo info = new TradeInfo()
            {
                MerchantID = "MS144603124",
                RespondType = "",
                TimeStamp = DateTime.Now.Ticks,
                Version = "2.0",
                MerchantOrderNo = "",
                Amt = 2000,
                ItemDesc = "",
                ReturnURL = "",
            };
            List<KeyValuePair<string, string>> tradeData = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(),

            };
            

            return Ok();
        }

    }
}
