﻿using Alcoholic.Extensions;
using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;

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

                    x = orderId;
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
        
        public GateWayInfoModel Payment(PaymentModel paymentInfo)
        {
            //paymentInfo.OrderId = "20221026193511";
            //int price = (from o in _db.OrderDetails where paymentInfo.OrderId == o.OrderId select o)
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
                ReturnURL = "https://www.google.com"
            };

            var hashKey = config["Payment:HashKey"];
            var hashIV = config["Payment:HashIV"];
            var aesString = aesService.AesEncrypt(Encoding.UTF8.GetBytes(tradeInfo.ToQueryString()),hashKey,hashIV);
            var shaString =hashService.GetHashHex($"HashKey={hashKey}&{aesString}&HashIV={hashIV}").ToUpper();

            return new GateWayInfoModel()
            {
                MerchantID = config["Payment:MerchantID"],
                TradeInfo = aesString,
                TradeSha = shaString,
                Version = "2.0"
            };
        }
    }
}
