﻿using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
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

            //判斷是否有session
            if (string.IsNullOrEmpty(sesStr))
            {
                var cartString = JsonConvert.SerializeObject(cartItem);
                HttpContext.Session.SetString("CartItem", cartString);
            }
            else
            {
                //判斷商品是否已在session中
                var sesItem = JsonConvert.DeserializeObject<List<CartItem>>(sesStr);
                for (int i = 0; i < cartItem.Count; i++)
                {
                    //var index = cartItem.FindIndex(c => c.Id.Equals(sesItem[i].Id));
                    //if (index != -1)
                    //{
                    //    sesItem[i].Qty += cartItem.Qty;
                    //    Console.WriteLine(index);
                    //}
                    if (sesItem.FindIndex(c => c.Id.Equals(cartItem[i].Id)) != -1)
                    {
                        sesItem.Where(c => c.Id == cartItem[i].Id).FirstOrDefault().Qty += cartItem[i].Qty;
                        Console.WriteLine(sesItem[0].Qty);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("asdbasdbasdb");
                    }

                }

            }
        }

            //Guid memberIdCookie = Guid.Parse(Request.Cookies["MemberID"]);

            //if (memberIdCookie != null)
            //{
            //    string? memberName = (from x in projectContext.Members
            //                         where x.MemberID == memberIdCookie
            //                         select x.MemberName).FirstOrDefault();
            //    ViewBag.memberName = memberName;
            //}
            //else
            //{
            //    return NotFound();
            //}
            //if (cartItem == null)
            //{
            //    return "fail";

            //}
            //return "KHGJLKKVKJ";

            //}

            [HttpPost]
        public IActionResult Success(string orderId)
        {

            string deskCookieSuccess = Request.Cookies["Desk"];
            string numberCookieSuccess = Request.Cookies["Number"];

            var order = (from x in projectContext.Orders where x.OrderId == orderId select x).FirstOrDefault();

            ViewBag.deskCookieSuccess = deskCookieSuccess;
            ViewBag.numberCookieSuccess = numberCookieSuccess;

            return View(order);
        }

        public IActionResult Total()
        {
            OrderTotalViewModel orderList = new OrderTotalViewModel();
            string memberIdCookie = Request.Cookies["MemberID"];
            string deskCookieTotal = Request.Cookies["Desk"];


            if (memberIdCookie != null)
            {
                string memberName = (from x in projectContext.Members
                                     where x.MemberID == Guid.Parse(memberIdCookie)
                                     select x).FirstOrDefault().MemberName;
                ViewBag.memberName = memberName;

                orderList.Orders = (from x in projectContext.Orders
                                    where x.Status == "N" && x.DeskNum == deskCookieTotal
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
