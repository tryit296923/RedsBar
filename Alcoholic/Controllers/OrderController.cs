﻿using Alcoholic.Models;
using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace Alcoholic.Controllers
{
    [Authorize(Roles = "member,Guest")]
    public class OrderController : Controller
    {
        private readonly db_a8de26_projectContext _db;
        private readonly IConfiguration config;
        private readonly AesService aesService;
        private readonly HashService hashService;

        public OrderController(db_a8de26_projectContext projectContext, IConfiguration config, AesService aesService, HashService hashService)
        {
            this._db = projectContext;
            this.config = config;
            this.aesService = aesService;
            this.hashService = hashService;
        }

        [HttpPost]
        public bool AddToCart([FromBody] List<CartItem> cartItem)
        {
            if (cartItem == null)
            {
                return false;
            }
            else
            {
                var sesStr = HttpContext.Session.GetString("CartItem");
                var addItem = new List<CartItem>();

                //判斷是否有session
                if (string.IsNullOrEmpty(sesStr))
                {
                    var cartString = JsonConvert.SerializeObject(cartItem);
                    HttpContext.Session.SetString("CartItem", cartString);
                }
                else
                {
                    var sesItem = JsonConvert.DeserializeObject<List<CartItem>>(sesStr);
                    //判斷商品是否已在session中
                    for (int i = 0; i < cartItem.Count; i++)
                    {
                        for (int S = 0; S < sesItem.Count; S++)
                        {
                            //重複產品
                            if (sesItem[S].Id == cartItem[i].Id)
                            {
                                sesItem[S].Qty += cartItem[i].Qty;
                                addItem.Add(sesItem[S]);
                                break;
                            }
                            //找完集合沒有重復產品
                            else if (sesItem[S].Id != cartItem[i].Id && S == sesItem.Count - 1)
                            {
                                sesItem.Add(cartItem[i]);
                                break;
                            }

                        }
                    }
                    //cartString為購物車字串，additem為物件
                    var cartString = JsonConvert.SerializeObject(sesItem);
                    HttpContext.Session.SetString("CartItem", cartString);
                }
                return true;

            }

        }
        public IActionResult Cart()
        {
            ReturnModel returnModel = new();
            //session取值是否可用
            var sesStr = HttpContext.Session.GetString("CartItem");
            if (string.IsNullOrEmpty(sesStr))
            {
                throw new Exception("sesStr notfound");
            }
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

            //HttpContext.Session.GetString("CartItem");

            //測試用
            //List<CartItem> cartItems = new List<CartItem>()
            //{
            //    new CartItem { Id = 1,Qty = 2 },
            //    new CartItem { Id = 2,Qty = 3 }
            //};

            //var x = projectContext.Discount.FirstOrDefault();

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

            return View(ovm_Cart);
        }

        [HttpPost]
        public IActionResult Success(string orderId)
        {
            var order = (from x in _db.Orders where x.OrderId == orderId select x).Select(x => new OrderViewModel
            {
                Desk = x.DeskNum,
                Number = x.Number,
                OrderId = x.OrderId,
                OrderTime = x.OrderTime,
            }).FirstOrDefault();
            // 送出訂單後清除session
            HttpContext.Session.SetString("Desk" ,"");
            HttpContext.Session.SetString("Number", "");
            HttpContext.Session.SetString("CartItem", "");
            return View(order);
        }

        public IActionResult Total()
        {
            string sMemberId = HttpContext.Session.GetString("MemberID");

            if (!Guid.TryParse(sMemberId, out var memberId))
            {
                throw new Exception("Guid is error");
            }

            var orderDetail = (from y in _db.Orders
                               where y.Status == "N" && y.MemberId == memberId
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

            return View(orderDetail);

            //OrderTotalViewModel orderList = new OrderTotalViewModel();
            //orderList.Orders = (from x in projectContext.Orders
            //                    where x.Status == "N" && x.DeskNum == sDeskTotal
            //                    select new OrderListViewModel
            //                    {
            //                        OrderId = x.OrderId,
            //                        MemberId = x.MemberId,
            //                        MemberName = x.Member.MemberName,
            //                        Number = x.Number,
            //                        OrderTime = x.OrderTime,
            //                        Desk = x.DeskNum,
            //                        Status = x.Status
            //                    }).ToList();

            //var temp = orderList.Orders.Select(x => x.OrderId).ToList();

            ////var product = _db.Products.FirstOrDefault();
            //orderList.Details = (from od in projectContext.OrderDetails
            //                     where temp.Contains(od.OrderId)
            //                     select new CartItem
            //                     {
            //                         OrderId = od.OrderId,
            //                         ProductName = od.Product.ProductName,
            //                         Path = od.Product.Images.FirstOrDefault().Path,
            //                         Qty = od.Quantity,
            //                         UnitPrice = od.UnitPrice,
            //                         DiscountAmount = (float)od.Discount,
            //                         Id = od.ProductId,
            //                     }).ToList();

            //return View(orderList);

        }
        public IActionResult OrderList()
        {
            return View();
        }
        public IActionResult Check(int total, string orderId)
        {
            var now = DateTime.Now.ToString("yyyy/MM/dd");
            string sMemberID = HttpContext.Session.GetString("MemberID");

            if (!Guid.TryParse(sMemberID, out var memberId))
            {
                throw new Exception("Guid is error");
            }
            var getOrder = _db.Orders.Where(x => x.MemberId == memberId && x.Status == "N").FirstOrDefault();
            var cartTotalList = getOrder.OrderDetails.Select(x => new CartTotal { ProductName = x.Product.ProductName, Quantity = x.Quantity })
                .GroupBy(x => x.ProductName, (k, v) => new CartTotal
                {
                    Quantity = v.Sum(x=>x.Quantity),
                    ProductName = k,
                }).ToList();

            OrderCheckViewModel orderCheckViewModel = new OrderCheckViewModel
            {
                Desk = getOrder.DeskNum,
                Number = getOrder.Number.ToString(),
                OrderId = orderId,
                OrderTime = now,
                Total = total,
                CartTotal = cartTotalList,
            };

            return View(orderCheckViewModel);
        }
        public IActionResult OnlinePayment(OnlinePaymentReturn onlinePaymentReturn)
        {
            if (onlinePaymentReturn.Status == "SUCCESS")
            {
                string hashKey = config["Payment:HashKey"];
                string hashIV = config["Payment:HashIV"];
                string decryptTradeInfo = aesService.DecryptAESHex(onlinePaymentReturn.TradeInfo, hashKey, hashIV);
                PaymentResult obj_PaymentResult = JsonConvert.DeserializeObject<PaymentResult>(decryptTradeInfo);

                var orderId = obj_PaymentResult.Result.MerchantOrderNo;

                ViewBag.orderId = orderId;
                return View("OnlinePaymentSucceed");
            }
            else
            {
                return View("OnlinePaymentFailed");
            }

        }
        public IActionResult FrontDeskCheckout()
        {
            return View();
        }
        public IActionResult Feedback(string OrderId)
        {
            ViewBag.order = OrderId;
            return View();
        }
    }
}
