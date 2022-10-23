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
                        if (sesItem[S].Id == cartItem[i].Id)
                        {
                            sesItem[S].Qty += cartItem[i].Qty;
                            Console.WriteLine($"產品ID{sesItem[i].Id}產品數量{sesItem[i].Qty}");
                            break;
                        }
                        //找完集合沒有重復產品
                        else if (sesItem[S].Id != cartItem[i].Id & S == sesItem.Count - 1)
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

            //var x = projectContext.Discount.FirstOrDefault();

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
            HttpContext.Session.SetString("testsession", JsonConvert.SerializeObject(cartItems));


            string stest = "";
            //若??前面為空，則用""取代
            stest = HttpContext.Session.GetString("testsession") ?? "";
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
                Desk = int.Parse(x.DeskNum),
                Number = x.Number.ToString(),
                OrderId = x.OrderId,
                OrderTime = x.OrderTime,
            }).FirstOrDefault();

            return View(order);
        }

        public IActionResult Total()
        {
            string sDeskTotal = HttpContext.Session.GetString("Desk");

            var orderDetail = (from y in projectContext.Orders where y.Status == "N" && y.DeskNum == sDeskTotal select y).Select(y => new OrderViewModel
            {
                Desk = int.Parse(y.DeskNum),
                Number = y.Number.ToString(),
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
                    Path = z.Product.Images.FirstOrDefault().Path
                }).ToList(),
            }).ToList();

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
        public IActionResult Check(int totalPrice)
        {
            var now = DateTime.Now.ToString("yyyy/MM/dd");
            string sDeskCheck = HttpContext.Session.GetString("Desk");
            string sNumberCheck = HttpContext.Session.GetString("Number");
            string sMemberID = HttpContext.Session.GetString("MemberID");

            ViewBag.orderTime = now;
            ViewBag.Number = sNumberCheck;
            ViewBag.Desk = sDeskCheck;
            ViewBag.totalPrice = totalPrice;

            var cartTotal = (from o in projectContext.OrderDetails
                             where o.Order.MemberId == Guid.Parse(sMemberID) && o.Order.Status == "N"
                             group o by new{ o.ProductId, o.Product.ProductName} into n
                             select new { item = n.Key, cnt = n.Count() }).ToList();
            return View(cartTotal);
        }
        public IActionResult FrontDeskCheckout()
        {
            return View();
        }
    }
}
