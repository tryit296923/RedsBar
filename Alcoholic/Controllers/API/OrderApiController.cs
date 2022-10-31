using Alcoholic.Extensions;
using Alcoholic.Hubs;
using Alcoholic.Models;
using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Data;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Alcoholic.Controllers.API
{
    [Authorize(Roles = "member,Guest,leader,mod,staff")]

    [Route("api/Order/[action]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly db_a8de26_projectContext _db;
        private readonly IConfiguration config;
        private readonly AesService aesService;
        private readonly HashService hashService;
        private readonly IHubContext<NotifyHub> hub;

        public OrderApiController(db_a8de26_projectContext projectContext,
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
        public IActionResult GetCartInfo()
        {
            //session取值是否可用
            var sesStr = HttpContext.Session.GetString("CartItem");
            Console.WriteLine(sesStr);
            string sMemberID = HttpContext.Session.GetString("MemberID");
            if (!Guid.TryParse(sMemberID, out var memberId))
            {
                //錯誤訊息待修改
                throw new Exception("Guid is error");
            }
            string memberName = "";
            if (sMemberID != null)
            {
                memberName = (from n in _db.Members
                              where n.MemberID == memberId
                              select n.MemberName).FirstOrDefault();
            }
            else
            {
                return NotFound();
            }

            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(sesStr);

            foreach (var cartItem in cartItems)
            {
                var product = _db.Products.Find(cartItem.Id);
                cartItem.ProductName = product.ProductName;
                cartItem.UnitPrice = product.UnitPrice;
                cartItem.DiscountAmount = product.Discount.DiscountAmount;
                cartItem.Path = product.Images.FirstOrDefault().Path;
            }
            //HttpContext.Session.SetString("123", "456");
            //var s = HttpContext.Session.GetString("123");
            HttpContext.Session.SetString("CartItem", JsonConvert.SerializeObject(cartItems));

            string stest = "";
            //若??前面為空，則用""取代
            stest = HttpContext.Session.GetString("CartItem") ?? "";
            //var temp = JsonConvert.DeserializeObject<List<CartItem>>(stest);
            var ovm_Cart = new OrderViewModel
            {
                MemberName = memberName,
                ItemList = cartItems,
            };

            return Ok(ovm_Cart);
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
                              where o.MemberId == Guid.Parse(sMemberID) && o.Status == "N" && o.DeskNum == sDesk
                              select o.OrderId).FirstOrDefault();
            try
            {
                int total = 0;
                if (orderdata != null)
                {
                    orderdata?.ItemList?.ForEach(x => total += Convert.ToInt32(Math.Round((float)(x.Qty * x.DiscountAmount * x.UnitPrice))));
                }
                if (db_OrderId == null)
                {
                    //分別存入Order, OrderDetail
                    var order = new Order
                    {
                        OrderId = orderId,
                        MemberId = Guid.Parse(sMemberID),
                        Number = int.Parse(sNumber),
                        OrderTime = Convert.ToDateTime(now),
                        DeskNum = sDesk,
                        Feedback = null,
                        Total = total,
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
                        //扣庫存
                        var prods = (from p in _db.Products
                                     join pm in _db.ProductsMaterials
                                     on p.ProductId equals pm.ProductId
                                     where pm.ProductId == item.Id
                                     select pm.Material).ToList();
                        foreach (Material material in prods)
                        {
                            var cosumption = (from pm in _db.ProductsMaterials where pm.MaterialId == material.MaterialId select pm.Consumption).FirstOrDefault();
                            material.Inventory = material.Inventory - orderDetail.Quantity * cosumption;
                            _db.Entry(material).State = EntityState.Modified;

                        }
                    }
                    db_OrderId = orderId;
                }
                else
                {
                    var oriOrder = (from t in _db.Orders where t.OrderId == db_OrderId select t).FirstOrDefault();
                    oriOrder.OrderTime = DateTime.Parse(now);
                    oriOrder.Total += total;
                    _db.Update(oriOrder);

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
                        //扣庫存
                        var prods = (from p in _db.Products
                                     join pm in _db.ProductsMaterials
                                     on p.ProductId equals pm.ProductId
                                     where pm.ProductId == item.Id
                                     select pm.Material).ToList();
                        foreach (Material material in prods)
                        {
                            var cosumption = (from pm in _db.ProductsMaterials where pm.MaterialId == material.MaterialId select pm.Consumption).FirstOrDefault();
                            material.Inventory = material.Inventory - orderDetail.Quantity * cosumption;
                            _db.Entry(material).State = EntityState.Modified;

                        }
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
        [HttpPost]
        public IActionResult SendOrderSucceed([FromBody] SearchHistOrder orderInfo)
        {
            var data = (from x in _db.Orders
                        where x.OrderId == orderInfo.OrderId
                        select x).Select(x => new OrderViewModel
                        {
                            Desk = x.DeskNum,
                            Number = x.Number,
                            OrderId = x.OrderId,
                            OrderTime = x.OrderTime,
                        }).FirstOrDefault();
            // 送出訂單後清除session
            HttpContext.Session.SetString("CartItem", "");
            return Ok(data);
        }
        public IActionResult GetTotalOrder()
        {
            string sMemberId = HttpContext.Session.GetString("MemberID");
            string sDesk = HttpContext.Session.GetString("Desk");

            if (!Guid.TryParse(sMemberId, out var memberId))
            {
                throw new Exception("Guid is error");
            }

            var orderDetail = (from y in _db.Orders
                               where y.Status == "N" && y.MemberId == memberId && y.DeskNum == sDesk
                               select y).Select(y => new OrderViewModel
                               {
                                   Desk = y.DeskNum,
                                   Number = y.Number,
                                   OrderId = y.OrderId,
                                   OrderTime = y.OrderTime,
                                   MemberName = y.Member.MemberName,
                                   Status = y.Status,
                                   ItemList = y.OrderDetails.Select(z => new CartItem
                                   {
                                       Id = z.ProductId,
                                       Qty = z.Quantity,
                                       ProductName = z.Product.ProductName,
                                       OrderId = z.OrderId,
                                       UnitPrice = z.UnitPrice,
                                       DiscountAmount = z.Product.Discount.DiscountAmount,
                                       Path = z.Product.Images.FirstOrDefault().Path,
                                       Sequence = z.Sequence,
                                   }).ToList(),
                               }).FirstOrDefault();
            return Ok(orderDetail);
        }
        public IActionResult GetCheckInfo()
        {
            var now = DateTime.Now.ToString("yyyy/MM/dd");
            string sMemberID = HttpContext.Session.GetString("MemberID");
            string sDesk = HttpContext.Session.GetString("Desk");

            if (!Guid.TryParse(sMemberID, out var memberId))
            {
                throw new Exception("Guid is error");
            }
            // TODO: 處理訪客ID相同的狀況??orderTime/desk
            var getOrder = _db.Orders.Where(x => x.MemberId == memberId && x.Status == "N" && x.DeskNum == sDesk).FirstOrDefault();
            var cartTotalList = getOrder.OrderDetails.Select(x => new CartTotal { ProductName = x.Product.ProductName, Quantity = x.Quantity })
                .GroupBy(x => x.ProductName, (k, v) => new CartTotal
                {
                    Quantity = v.Sum(x => x.Quantity),
                    ProductName = k,
                }).ToList();
            var total = 0;
            if (getOrder != null)
            {
                total = (int)getOrder.Total;
            }
            OrderCheckViewModel orderCheckViewModel = new OrderCheckViewModel
            {
                Desk = getOrder.DeskNum,
                Number = getOrder.Number.ToString(),
                OrderId = getOrder.OrderId,
                OrderTime = now,
                Total = total,
                CartTotal = cartTotalList,
            };
            return Ok(orderCheckViewModel);
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
                Status = 200,
                Url = $"{Request.Scheme}://{Request.Host}/Home/Index",
            };
            _db.Add(feedback);
            _db.SaveChanges();
            return Ok(returnModel);
        }

        // 後台顯示今日訂單
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
                             Total = null
                         }).ToList();
            return Ok(order);
        }

        // 後臺查詢歷史訂單
        [HttpPost]
        public IActionResult SearchHistOrders([FromBody] SearchHistOrder histOrder)
        {
            var histOrderInfo = (from histOd in _db.Orders
                                 where histOd.OrderId == histOrder.OrderId
                                 select new BCOrder
                                 {
                                     OrderId = histOrder.OrderId,
                                     Desk = histOd.DeskNum,
                                     MemberName = histOd.Member.MemberName,
                                     Number = histOd.Number,
                                     Status = histOd.Status,
                                     Total = histOd.Total,
                                     OrderTime = (histOd.OrderTime).ToString(),
                                 }).ToList();
            return Ok(histOrderInfo);
        }
        // 後臺按下確認結單
        [HttpPost]
        public IActionResult FinishPayment([FromBody] SearchHistOrder checkConfirm)
        {
            var order = (from od in _db.Orders where od.OrderId == checkConfirm.OrderId select od).FirstOrDefault();
            var orderDetail = (from x in _db.OrderDetails
                               where x.OrderId == checkConfirm.OrderId
                               group x by x.ProductId into o
                               select new { ProductId = o.Key, Total = Math.Round(o.Sum(x => x.Quantity * x.UnitPrice * x.Discount)) }).ToArray();

            var totalPrice = 0;

            foreach (var item in orderDetail)
            {
                totalPrice += (int)item.Total;
            }

            if (order != null)
            {
                order.Status = "Y";
                order.Total = totalPrice;
                var desk = _db.DeskInfo.Where(x => x.Desk == order.DeskNum).FirstOrDefault();
                if (desk != null)
                {
                    desk.StartTime = null;
                    desk.Occupied = 0;
                }
                _db.SaveChanges();
                HttpContext.Session.SetString("Desk", "");
                HttpContext.Session.SetString("Number", "");
                hub.Clients.All.SendAsync("OK", desk.Desk);
                return Ok(true);
            }

            return Ok(false);
        }

        // 櫃台結帳頁面出示
        public IActionResult GetOrder()
        {
            string sMemberID = HttpContext.Session.GetString("MemberID");
            string sDesk = HttpContext.Session.GetString("Desk");

            if (!Guid.TryParse(sMemberID, out var memberId))
            {
                throw new Exception("Guid is error");
            }
            var fd_checkOrder = (from o in _db.Orders
                                 where o.MemberId == memberId && o.Status == "N" && o.DeskNum == sDesk
                                 select o).FirstOrDefault();
            FrontDeskCheckPage frontDeskCheckPage = new FrontDeskCheckPage
            {
                OrderId = fd_checkOrder.OrderId,
                Desk = fd_checkOrder.DeskNum,
                OrderTime = fd_checkOrder.OrderTime,
            };
            return Ok(frontDeskCheckPage);
        }
        //測試SignalR
        //public async Task<IActionResult> TestDesk(string desk)
        //{
        //    await hub.Clients.All.SendAsync("OK", desk);
        //    return Content(desk);
        //}
        // 後臺查詢歷史訂單之詳細資料
        [HttpPost]
        public IActionResult ShowDetailList([FromBody] SearchHistOrder histOrderDetail)
        {
            var temp = Request;
            var h_OrderDetail = (from h_OD in _db.OrderDetails
                                 where h_OD.OrderId == histOrderDetail.OrderId
                                 select new CartItem
                                 {
                                     Path = h_OD.Product.Images.FirstOrDefault().Path,
                                     DiscountAmount = h_OD.Discount,
                                     ProductName = h_OD.Product.ProductName,
                                     Qty = h_OD.Quantity,
                                     UnitPrice = h_OD.UnitPrice
                                 }).ToList();
            return Ok(h_OrderDetail);
        }
    }
}
