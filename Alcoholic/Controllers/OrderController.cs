using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Alcoholic.Controllers
{
    public class OrderController : Controller
    {
        private readonly db_a8de26_projectContext projectContext;

        public OrderController(db_a8de26_projectContext projectContext)
        {
            this.projectContext = projectContext;
        }
        public IActionResult Order()
        {
            return RedirectToAction("Cart", "Order");
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
            //session取值是否可用
            var sesStr = HttpContext.Session.GetString("CartItem");
            Console.WriteLine(sesStr);
            string sMemberID = HttpContext.Session.GetString("MemberID");
            if (!Guid.TryParse(sMemberID,out var memberId))
            {
                //錯誤訊息待修改
                throw new Exception("Guid is error");
            }
            string memberName = "";
            if (sMemberID != null)
            {
                memberName = (from n in projectContext.Members
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
                var product = projectContext.Products.Find(cartItem.Id);
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
            var order = (from x in projectContext.Orders where x.OrderId == orderId select x).Select(x => new OrderViewModel
            {
                Desk = x.DeskNum,
                Number = x.Number,
                OrderId = x.OrderId,
                OrderTime = x.OrderTime,
            }).FirstOrDefault();

            return View(order);
        }

        public IActionResult Total()
        {
            string sDeskTotal = HttpContext.Session.GetString("Desk");

            var orderDetail = (from y in projectContext.Orders
                               where y.Status == "N" && y.DeskNum == sDeskTotal
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
            string sDeskCheck = HttpContext.Session.GetString("Desk");
            string sNumberCheck = HttpContext.Session.GetString("Number");
            string sMemberID = HttpContext.Session.GetString("MemberID");

            if (!Guid.TryParse(sMemberID, out var memberId))
            {
                throw new Exception("Guid is error");
            }
            var getOrder = projectContext.Orders.Where(x => x.MemberId == memberId && x.Status == "N").FirstOrDefault();
            var cartTotalList = getOrder.OrderDetails.Select(x => new { x.Product.ProductName, x.ProductId })
                .GroupBy(x => new { x.ProductId, x.ProductName }, (k, v) => new CartTotal
                {
                    Count = v.Count(),
                    IdNamePair = new CartIdNamePair() { ProductName = k.ProductName, ProductId = k.ProductId }
                }).ToList();
            
            OrderCheckViewModel orderCheckViewModel = new OrderCheckViewModel
            {
                Desk = sDeskCheck,
                Number = sNumberCheck,
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
    }
}
