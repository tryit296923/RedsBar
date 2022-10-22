using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

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
            var addItem = new List<CartItem>();

            //判斷是否有session
            if (string.IsNullOrEmpty(sesStr))
            {
                var cartString = JsonConvert.SerializeObject(cartItem);
                HttpContext.Session.SetString("CartItem", cartString);
                addItem = cartItem;
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
                        if (sesItem[S].Id == cartItem[i].Id) {
                            sesItem[S].Qty += cartItem[i].Qty;
                            Console.WriteLine($"產品ID{sesItem[i].Id}產品數量{sesItem[i].Qty}");
                        break;
                        }
                        //找完集合沒有重復產品
                        else if(sesItem[S].Id != cartItem[i].Id & S==sesItem.Count-1)
                        {
                            Console.WriteLine("新商品");
                            addItem.Add(cartItem[i]);
                            Console.WriteLine($"產品ID{sesItem[i].Id}產品數量{sesItem[i].Qty}");
                            break;
                        }
                        else
                        {
                          Console.WriteLine("找");
                          
                        }
                        }
                }
            }
            foreach (var item in addItem)
            {
                //additem為購物車物件
                Console.WriteLine($"ID{item.Id}數量{item.Qty}");
            }
            
            
        }
        public IActionResult Cart()
        {
            
            string sMemberID = HttpContext.Session.GetString("MemberID");
            string memberName = "";
            if (sMemberID != null)
            {
                memberName = (from n in projectContext.Members
                                     where n.MemberID == Guid.Parse(sMemberID)
                                     select n.MemberName).FirstOrDefault();
            }
            else
            {
                return NotFound();
            }

            //HttpContext.Session.GetString("CartItem");

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
                cartItem.DiscountAmount = 0.8;  //測試
                //cartItem.Path = product.Images;
            }

            //HttpContext.Session.SetString("123", "456");
            //var s = HttpContext.Session.GetString("123");
            HttpContext.Session.SetString("testsession", JsonConvert.SerializeObject(cartItems));


            string stest = "";
            //若??前面為空，則用""取代
            stest = HttpContext.Session.GetString("testsession")??"";
            //var temp = JsonConvert.DeserializeObject<List<CartItem>>(stest);
            var ovm = new OrderViewModel
            {
                MemberName = memberName,
                ItemList = cartItems,
            };

            return View(ovm);
        }

        [HttpPost]
        public IActionResult Success(string orderId)
        {
            string sDeskSuccess = HttpContext.Session.GetString("Desk");
            string sNumberSuccess = HttpContext.Session.GetString("Number");
           
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
                                     select new CartItem
                                     {
                                         OrderId = od.OrderId,
                                         ProductName = od.Product.ProductName,
                                         //Path = od.Product.Images,
                                         Qty = od.Quantity,
                                         UnitPrice = od.UnitPrice,
                                         DiscountAmount = od.Discount,
                                         Id = od.ProductId,
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
