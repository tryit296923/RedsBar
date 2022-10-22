using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
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
        public void AddToCart([FromBody] List<CartItem> cartItem)
        {
            var sesStr = HttpContext.Session.GetString("CartItem");
            var abc = System.Text.Json.JsonSerializer.Serialize(sesStr);

            //判斷是否有session
            if (string.IsNullOrEmpty(sesStr))
            {
                var cartString = System.Text.Json.JsonSerializer.Serialize(cartItem);
                HttpContext.Session.SetString("CartItem", cartString);
            }
            else
            {
                //var abc = System.Text.Json.JsonSerializer.Serialize(sesStr);

            }
            //判斷商品是否已在session中              


            //for (int i = 0; i < abc.Length; i++)
            //{
            //    if(abc[i].Id
            //}

            //int index = abc.FindIndex(c => c.Id.Equals(.));
            //if (index != -1)
            //{
            //    cart[index].Amount += item.Amount;
            //    cart[index].SubTotal += item.SubTotal;
            //}
            //else
            //{
            //    cart.Add(item);
            //}

            //}
        }
        public IActionResult Cart()
        {
            //測試用
            List<CartItem> cartItems = new List<CartItem>()
            {
                new CartItem { Id = 1,Qty = 2 },
                new CartItem { Id = 2,Qty = 3 }
            };

            foreach (var cartItem in cartItems)
            {
                var product = projectContext.Products.Find(cartItem.Id);
                cartItem.ProductName = product.ProductName;
                cartItem.UnitPrice = product.UnitPrice;
                //...
            }

            HttpContext.Session.SetString("testsession", JsonConvert.SerializeObject("cartItems"));
            var stest = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString("testsession"));

            return View();
            //if(cartItem == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    return View();
            //}

            //string sMemberID = HttpContext.Session.GetString("MemberID");
            ////Guid memberIdCookie = Guid.Parse(Request.Cookies["MemberID"]);
            ///
        }

        [HttpPost]
        public IActionResult Success(string orderId)
        {
            string sDeskSuccess = HttpContext.Session.GetString("Desk");
            string sNumberSuccess = HttpContext.Session.GetString("Number");
            //string deskCookieSuccess = Request.Cookies["Desk"];
            //string numberCookieSuccess = Request.Cookies["Number"];

            var order = (from x in projectContext.Orders where x.OrderId == orderId select x).FirstOrDefault();

            ViewBag.deskSessionSuccess = sDeskSuccess;
            ViewBag.numberSessionSuccess = sNumberSuccess;

            return View(order);
        }

        public IActionResult Total()
        {
            OrderTotalViewModel orderList = new OrderTotalViewModel();
            string sMemberIDTotal = HttpContext.Session.GetString("MemberID");
            string sDeskTotal = HttpContext.Session.GetString("Desk");
            //string memberIdCookie = Request.Cookies["MemberID"];
            //string deskCookieTotal = Request.Cookies["Desk"];


            if (sMemberIDTotal != null)
            {
                string memberName = (from x in projectContext.Members
                                     where x.MemberID == Guid.Parse(sMemberIDTotal)
                                     select x).FirstOrDefault().MemberName;
                ViewBag.memberName = memberName;

                orderList.Orders = (from x in projectContext.Orders
                                    where x.Status == "N" && x.DeskNum == sDeskTotal
                                    select new OrderListViewModel
                                    {
                                        OrderId = x.OrderId,
                                        MemberId = x.MemberId,
                                        Number = x.Number,
                                        OrderTime = x.OrderTime,
                                        DeskNum = x.DeskNum,
                                        Status = x.Status
                                    }).ToList();

                var temp = orderList.Orders.Select(x => x.OrderId).ToList();

                //var product = _db.Products.FirstOrDefault();
                orderList.Details = (from od in projectContext.OrderDetails
                                     where temp.Contains(od.OrderId)
                                     select new DetailViewModel
                                     {
                                         OrderId = od.OrderId,
                                         ProductName = od.Product.ProductName,
                                         //Path = od.Product.Images,
                                         Quantity = od.Quantity,
                                         UnitPrice = od.UnitPrice,
                                         Discount = od.Discount,
                                         ProductId = od.ProductId,
                                     }).ToList();

                return View(orderList);
            }
            else
            {
                return NotFound();
            }

        }
        public IActionResult OrderList()
        {
            return View();
        }
        public IActionResult Check()
        {
            var now = DateTime.Now.ToString("yyyy/MM/dd");
            ViewBag.orderTime = now;
            return View();
        }

    }
}
