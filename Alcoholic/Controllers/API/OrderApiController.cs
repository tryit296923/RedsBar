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
            var orderId = DateTime.Now.ToString("yyyyMMddHHmm") + sDesk;
            var now = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            var x = (from o in _db.Orders
                     where o.MemberId == Guid.Parse(sMemberID) && o.Status == "N"
                     select o.OrderId).FirstOrDefault();
            try
            {
                if (x == null)
                {
                    //分別存入Order, OrderDetail
                    var order = new Order
                    {
                        MemberId = Guid.Parse(sMemberID),
                        OrderId = orderId,
                        Number = int.Parse(sNumber),
                        DeskNum = sDesk,
                        OrderTime = Convert.ToDateTime(now),
                        Feedback = null,
                        Total = null,
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
                            Sequence = 1,
                        };
                        _db.Add(orderDetail);
                    }
                }
                else
                {
                    var newTime = (from t in _db.Orders where t.OrderId == x select t).FirstOrDefault();
                    newTime.OrderTime = DateTime.Parse(now);
                    _db.Update(newTime);

                    var seq = (from s in _db.OrderDetails where s.OrderId == x orderby s.Sequence select s.Sequence).LastOrDefault() + 1;
                    foreach (var item in orderdata.ItemList)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = x,
                            ProductId = item.Id,
                            Quantity = item.Qty,
                            UnitPrice = item.UnitPrice ?? 0,
                            Discount = item.DiscountAmount ?? 0,
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
            //加密金鑰
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsetting.json").Build();
            string hashKey = config.GetSection("HashKey").Value;
            string hashIV = config.GetSection("HashIV").Value;

            TradeInfo info = new TradeInfo();

            //金流串接資料
            info.MerchantID = config.GetSection("MerchantID").Value;
            info.Version = "2.0";
            string tradeSha = "";
            string tradeInfo = "";

            //tradeInfo所需資料
            info.RespondType = "JSON";
            info.TimeStamp = DateTime.Now.Ticks.ToString();
            info.MerchantOrderNo = "";
            info.ItemDesc = "";
            info.Amt = "";
            info.ReturnURL = "http://127.0.0.1:5501/Pages/ordering_checksuccessed.html";

            //model轉成List<KeyValuePair<string,string>>
            List<KeyValuePair<string, string>> tradeData = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("MerchantID",info.MerchantID),
                new KeyValuePair<string, string>("RespondType",info.RespondType),
                new KeyValuePair<string, string>("TimeStamp",info.TimeStamp),
                new KeyValuePair<string, string>("Version",info.Version),
                new KeyValuePair<string, string>("MerchantOrderNo",info.MerchantOrderNo),
                new KeyValuePair<string, string>("Amt",info.Amt),
                new KeyValuePair<string, string>("ItemDesc",info.ItemDesc),
                new KeyValuePair<string, string>("ReturnURL",info.ReturnURL),
            };
            //轉換成key=Value
            var tradeQueryPara = string.Join("&",tradeData.Select(x=>$"{x.Key}={x.Value}"));
            //tradeInfo加密(AES)
            

            return Ok();
        }

    }
}
