using Alcoholic.Extensions;
using Alcoholic.Models;
using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Net;
using System.Text;

namespace Alcoholic.Controllers.API
{
    [Route("api/Order/[action]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly db_a8de26_projectContext _db;
        private readonly IConfiguration config;
        private readonly AesService aesService;
        private readonly HashService hashService;

        public OrderApiController(db_a8de26_projectContext projectContext, IConfiguration config, AesService aesService, HashService hashService)
        {
            this._db = projectContext;
            this.config = config;
            this.aesService = aesService;
            this.hashService = hashService;
        }
        [HttpPost]
        public IActionResult Confirm([FromBody] OrderViewModel orderdata)
        {
            string sMemberID = HttpContext.Session.GetString("MemberID");
            string sDesk = HttpContext.Session.GetString("Desk");
            string sNumber = HttpContext.Session.GetString("Number");
            var orderId = DateTime.Now.ToString("yyyyMMddHHmm") + sDesk;
            var now = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            var db_OrderId = (from o in _db.Orders
                              where o.MemberId == Guid.Parse(sMemberID) && o.Status == "N"
                              select o.OrderId).FirstOrDefault();
            try
            {
                if (db_OrderId == null)
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

                    db_OrderId = orderId;
                }
                else
                {
                    var newTime = (from t in _db.Orders where t.OrderId == db_OrderId select t).FirstOrDefault();
                    newTime.OrderTime = DateTime.Parse(now);
                    _db.Update(newTime);

                    var seq = (from s in _db.OrderDetails where s.OrderId == db_OrderId orderby s.Sequence select s.Sequence).LastOrDefault() + 1;
                    foreach (var item in orderdata.ItemList)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = db_OrderId,
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

                return new JsonResult(new { Status = 1, Message = "Save Success", OrderId = db_OrderId });
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

        // 付款前接收資訊
        [HttpPost]
        public GateWayInfoModel Payment([FromBody] PaymentModel paymentInfo)
        {
            var details = _db.OrderDetails.Where(x => x.OrderId == paymentInfo.OrderId);
            var productName = details.Include(x => x.Product).Select(x => x.Product.ProductName);
            var price = details.Select(x => (x.UnitPrice * x.Discount) * x.Quantity).Sum();
            TradeInfo tradeInfo = new TradeInfo
            {
                MerchantID = config["Payment:MerchantID"],
                RespondType = "JSON",
                Version = "2.0",
                TimeStamp = DateTime.Now.Ticks.ToString(),
                MerchantOrderNo = paymentInfo.OrderId,
                Amt = price.ToString(),
                ItemDesc = String.Join(",", productName),
                AndroidPay = paymentInfo.PayType.ToLower() == "googlepay" ? "1" : null,
                Credit = paymentInfo.PayType.ToLower() == "credit" ? "1" : null,
                LinePay = paymentInfo.PayType.ToLower() == "linepay" ? "1" : null,
                ReturnURL = "https://ff49-36-225-197-20.jp.ngrok.io/api/Order/GetPaymentReturnData",               
            };

            var hashKey = config["Payment:HashKey"];
            var hashIV = config["Payment:HashIV"];
            var aesString = aesService.AesEncrypt(Encoding.UTF8.GetBytes(tradeInfo.ToQueryString()), hashKey, hashIV);
            var shaString = hashService.GetHashHex($"HashKey={hashKey}&{aesString}&HashIV={hashIV}").ToUpper();

            return new GateWayInfoModel()
            {
                MerchantID = config["Payment:MerchantID"],
                TradeInfo = aesString,
                TradeSha = shaString,
                Version = "2.0"
            };
        }
        // 付款後ReturnUrl，接收回傳資料
        [HttpPost]      
        public IActionResult GetPaymentReturnData([FromForm] OnlinePaymentReturn returnData)
        {
            // 測試用參數
            //var aaa = "{\"Status\":\"SUCCESS\",\"Message\":\"\\u6388\\u6b0a\\u6210\\u529f\",\"Result\":{\"MerchantID\":\"MS144603124\",\"Amt\":48800,\"TradeNo\":\"22102721422172843\",\"MerchantOrderNo\":\"20221027070512\",\"RespondType\":\"JSON\",\"IP\":\"114.137.244.87\",\"EscrowBank\":\"HNCB\",\"ItemDesc\":\"\\u611b\\u723e\\u862d\\u5496\\u5561 Irish Coffee,\\u87ba\\u7d72\\u8d77\\u5b50 Screwdriver,\\u53e4\\u5178\\u96de\\u5c3e\\u9152 Old Fashioned,\\u8840\\u8165\\u746a\\u9e97 Bloody Mary,\\u7434\\u8cbb\\u53f8 Gin Fizz,\\u67ef\\u5922\\u6ce2\\u4e39 Cosmopolitan,\\u4e7e\\u99ac\\u4e01\\u5c3c Dry Martini,\\u9577\\u5cf6\\u51b0\\u8336 Long Island Iced Tea,\\u746a\\u683c\\u8389\\u7279 Margarita,\\u83ab\\u897f\\u591a Mojito,\\u5496\\u5561\\u5c3c\\u683c\\u7f85\\u5c3c Coffee Negroni,\\u840a\\u59c6\\u4f0f\\u7279\\u52a0 Vodka Lime\",\"PaymentType\":\"CREDIT\",\"PayTime\":\"2022-10-27 21:42:21\",\"RespondCode\":\"00\",\"Auth\":\"394437\",\"Card6No\":\"400022\",\"Card4No\":\"1111\",\"Exp\":\"2911\",\"TokenUseStatus\":0,\"InstFirst\":0,\"InstEach\":0,\"Inst\":0,\"ECI\":\"\",\"PaymentMethod\":\"CREDIT\",\"AuthBank\":\"KGI\"}}";
            //PaymentResult obj_PaymentResult = JsonConvert.DeserializeObject<PaymentResult>(aaa);
            //return;

            // var aaa = HttpContext.Request.Form; 檢查參數是否成功回傳
            string hashKey = config["Payment:HashKey"];
            string hashIV = config["Payment:HashIV"];

            string r_Status = returnData.Status;
            string r_MerchantID = returnData.MerchantID;
            string r_TradeInfo = returnData.TradeInfo;
            string r_TradeSha = returnData.TradeSha;
            string r_Version = returnData.Version;
            
            // AES解密
            string decryptTradeInfo = aesService.DecryptAESHex(r_TradeInfo, hashKey, hashIV);
            PaymentResult obj_PaymentResult = JsonConvert.DeserializeObject<PaymentResult>(decryptTradeInfo);

            var orderId = obj_PaymentResult.Result.MerchantOrderNo;
            var orderTotal = obj_PaymentResult.Result.Amt;

            // 付款成功與否 
            if (r_Status == "SUCCESS")
            {
                var order = _db.Orders.Where(x => x.OrderId == orderId).FirstOrDefault();
                if(order!= null)
                {
                    order.Status = "Y";
                    order.Total = int.Parse(orderTotal);
                    var desk =_db.DeskInfo.Where(x => x.Desk == order.DeskNum).FirstOrDefault();
                    if(desk != null)
                    {
                        desk.StartTime = null;
                        desk.Occupied = 0;
                    }
                    _db.SaveChanges();                   
                }
            }
            else
            {
                
            }
            OnlinePaymentReturn onlinePaymentReturn = new OnlinePaymentReturn
            {
                MerchantID = r_MerchantID,
                Status = r_Status,
                TradeInfo = r_TradeInfo,
                TradeSha = r_TradeSha,
                Version = r_Version,
            };
            return RedirectToAction("OnlinePayment", "Order", onlinePaymentReturn);
        }

        [HttpPost]
        public IActionResult FeedbackMember([FromBody] FeedbackIdModel data)
        {
            var order = (from x in _db.Orders
                         where x.OrderId == data.Id
                         select x).FirstOrDefault();
            var member = (from y in _db.Members
                          where y.MemberID == order.MemberId
                          select new 
                          {
                              OrderId = data.Id,
                              MemberName = y.MemberName,
                              Email = y.Email,
                              Age = y.Age,
                          }).FirstOrDefault();
            return Ok(member);
        }

        [HttpPost]
        public IActionResult FeedBackAll([FromBody] FeedBackAllModel data)
        {
            Feedback feedback = new Feedback()
            {
                OrderId = data.OrderId,
                FeedbackName = data.FeedbackName,
                Email = data.Email,
                Age = data.Age.ToString(),
                Frequency = data.Freq,
                Environment = data.Environment,
                Serve = data.Serve,
                Dish = data.Dish,
                Price = data.Price,
                Overall = data.Overall,
                Suggestion = data.Suggestion,
            };
            ReturnModel returnModel = new()
            {
                Status=200,
                Url=$"{Request.Scheme}://{Request.Host}/Home/Index",
            };
            _db.Add(feedback);
            _db.SaveChanges();
            return Ok(returnModel);
        }

        public IActionResult GetTodayOrders()
        {
            var order = (from od in _db.Orders
                         where od.OrderTime.Date >= DateTime.Today && od.OrderTime.Date <= DateTime.Today.AddDays(1)
                         select new BCOrder
                         {
                             OrderId = od.OrderId,
                             Desk = od.DeskNum,
                             MemberName = od.Member.MemberName,
                             Number = od.Number,
                             Status = od.Status,
                         }).ToList();
            return Ok(order);
        }

        //還沒寫...
        //public IActionResult SearchHistOrders
        //{
        //    return OK();
        //}
    }
}
