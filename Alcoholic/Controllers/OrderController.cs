using Alcoholic.Models.Entities;
using Alcoholic.Models.ViewModels;
using Alcoholic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

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
        public IActionResult Cart(Member memberData)
        {
            Guid memberIdCookie = Guid.Parse(Request.Cookies["MemberID"]);

            if (memberIdCookie != null)
            {
                string memberName = (from x in projectContext.Members
                                     where x.MemberID == memberIdCookie
                                     select x).FirstOrDefault().MemberName;
                //string memberName = "Matt";
                ViewBag.memberName = memberName;
            }
            else
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost]
        public IActionResult Confirm([FromBody] OrderViewModel orderdata)
        {
            Guid memberIdCookie = Guid.Parse(Request.Cookies["MemberID"]);
            string numberCookie = Request.Cookies["Number"];
            string deskCookie = Request.Cookies["Desk"];

            var now = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            var orderId = Guid.NewGuid();

            try
            {
                //分別存入Order, OrderDetail
                var order = new Order
                {
                    MemberId = memberIdCookie,
                    OrderId = orderId,
                    Number = int.Parse(numberCookie),
                    DeskNum = deskCookie,
                    OrderTime = Convert.ToDateTime(now),
                };
                projectContext.Add(order);

                foreach (var item in orderdata.ItemList)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = orderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Discount = item.DiscountAmount,
                        Total = Convert.ToInt32(item.Quantity * item.UnitPrice * item.DiscountAmount),
                    };
                    projectContext.Add(orderDetail);
                }
                projectContext.SaveChanges();

                return new JsonResult(new { Status = 1, Message = "Save Success", OrderId = orderId });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Status = 2, Message = "Save Fail" });
            }

        }


        [HttpPost]
        public IActionResult Success(string orderId)
        {

            string deskCookie = Request.Cookies["Desk"];
            string numberCookie = Request.Cookies["Number"];

            var order = (from x in projectContext.Orders where x.OrderId == Guid.Parse(orderId) select x).FirstOrDefault();

            ViewBag.deskCookie = deskCookie;
            ViewBag.numberCookie = numberCookie;

            return View(order);
        }

        public IActionResult Total()
        {
            OrderListViewModel orderList = new OrderListViewModel();
            string memberIdCookie = Request.Cookies["MemberID"];
            string deskCookie = Request.Cookies["Desk"];


            if (memberIdCookie != null)
            {
                string memberName = (from x in projectContext.Members
                                     where x.MemberID == Guid.Parse(memberIdCookie)
                                     select x).FirstOrDefault().MemberName;
                ViewBag.memberName = memberName;

                orderList.Orders = (from x in projectContext.Orders
                                    where x.Status == "N" && x.DeskNum == deskCookie
                                    select x).ToList();
                var temp = orderList.Orders.Select(x => x.OrderId).ToList();

                //var product = _db.Products.FirstOrDefault();
                orderList.Details = (from od in projectContext.OrderDetails
                                     where temp.Contains(od.OrderId)
                                     select new OrderDetailViewModel
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
