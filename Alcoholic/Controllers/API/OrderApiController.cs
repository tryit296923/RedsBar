using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;

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
            Guid memberIdCookie = Guid.Parse(Request.Cookies["MemberID"]);
            string numberCookie = Request.Cookies["Number"];
            string deskCookie = Request.Cookies["Desk"];

            var now = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            var orderId = DateTime.Now.ToString("yyyyMMddHHmm") + deskCookie;

            try
            {
                //分別存入Order, OrderDetail
                var order = new Order
                {
                    MemberId = memberIdCookie,
                    OrderId = orderId,
                    Number = int.Parse(numberCookie),
                    DeskNum = deskCookie,
                    OrderTime = Convert.ToDateTime(now),
                    Feedback = null
                };
                _db.Add(order);

                foreach (var item in orderdata.ItemList)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = orderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Discount = item.DiscountAmount,
                        Total = Convert.ToInt32(item.Quantity * item.UnitPrice * item.DiscountAmount),
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
    }
}
