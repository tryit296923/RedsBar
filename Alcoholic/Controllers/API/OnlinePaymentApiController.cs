using Alcoholic.Models.DTO;
using Alcoholic.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using Alcoholic.Hubs;
using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Alcoholic.Extensions;

namespace Alcoholic.Controllers.API
{
    [Route("api/OnlinePayment/[action]")]
    [ApiController]
    public class OnlinePaymentApiController : Controller
    {
        private readonly db_a8de26_projectContext _db;
        private readonly IConfiguration config;
        private readonly AesService aesService;
        private readonly HashService hashService;
        private readonly IHubContext<NotifyHub> hub;

        public OnlinePaymentApiController(db_a8de26_projectContext projectContext,
            IConfiguration config,
            AesService aesService,
            HashService hashService,
            IHubContext<NotifyHub> hub)
        {
            this._db = projectContext;
            this.config = config;
            this.aesService = aesService;
            this.hashService = hashService;
            this.hub = hub;
        }

        // 付款前接收資訊
        [HttpPost]
        public GateWayInfoModel Payment([FromBody] PaymentModel paymentInfo)
        {
            var details = _db.OrderDetails.Where(x => x.OrderId == paymentInfo.OrderId);
            var productName = details.Include(x => x.Product).Select(x => x.Product.ProductName);
            var totalPrice = (from p in _db.Orders where p.OrderId == paymentInfo.OrderId select p.Total).FirstOrDefault();
            //var price = details.Select(x => (x.UnitPrice * x.Discount) * x.Quantity).Sum();
            TradeInfo tradeInfo = new TradeInfo
            {
                MerchantID = config["Payment:MerchantID"],
                RespondType = "JSON",
                Version = "2.0",
                TimeStamp = DateTime.Now.Ticks.ToString(),
                MerchantOrderNo = paymentInfo.OrderId,
                Amt = totalPrice.ToString(),
                ItemDesc = String.Join(",", productName),
                AndroidPay = paymentInfo.PayType.ToLower() == "googlepay" ? "1" : null,
                Credit = paymentInfo.PayType.ToLower() == "credit" ? "1" : null,
                LinePay = paymentInfo.PayType.ToLower() == "linepay" ? "1" : null,
                ReturnURL = "http://tryit296923-001-site1.btempurl.com/api/OnlinePayment/GetPaymentReturnData",
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

            var temp = User.Claims;
            var temp2 = User.Identities;
            var temp3 = User.Identity;

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
                if (order != null)
                {
                    order.Status = "Y";
                    order.Total = int.Parse(orderTotal);
                    var desk = _db.DeskInfo.Where(x => x.Desk == order.DeskNum).FirstOrDefault();
                    if (desk != null)
                    {
                        desk.StartTime = null;
                        desk.Occupied = 0;
                    }
                    _db.SaveChanges();

                    HttpContext.Session.SetString("Desk", "");
                    HttpContext.Session.SetString("Number", "");

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, order.Member.MemberName),
                    };
                    if (order.Member.MemberAccount == "guestonly123")
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Guest"));
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "member"));
                    }
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
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

    }
}
