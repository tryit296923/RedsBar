using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
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
        public IActionResult Cart()
        {
            
            List<CartItem> cartItems = new List<CartItem>()
            {
                new CartItem { Id = 1,Qty = 2 },
                new CartItem { Id = 2,Qty = 3 }
            };

            foreach(var cartItem in cartItems)
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

            //if (sMemberID != null)
            //{
            //    string? memberName = (from x in projectContext.Members
            //                         where x.MemberID == Guid.Parse(sMemberID)
            //                          select x.MemberName).FirstOrDefault();
            //    ViewBag.memberName = memberName;
            //}
            //else
            //{
            //    return NotFound();
            //}
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
