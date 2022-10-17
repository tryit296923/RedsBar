using Alcoholic.Models.Entities;
using Alcoholic.Models.ViewModels;
using Alcoholic.Services;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Cart(Member memberData)
        {
            string memberIdCookie = Request.Cookies["MemberID"];

            if(memberIdCookie != null)
            {
                //string memberName = (from x in projectContext.Members
                //                     where x.MemberID == memberIdCookie
                //                     select x).FirstOrDefault().MemberName;
                string memberName = "Matt";
                ViewBag.memberName = memberName;
            }
            else
            {
                return NotFound();
            }
           
            return View();
        }


        public IActionResult Success()
        {
            string deskCookie = Request.Cookies["Desk"];
            string numberCookie = Request.Cookies["Number"];

            var now = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            var orderId = DateTime.Now.ToString("yyyyMMddHHmmss");

            ViewBag.deskCookie = deskCookie;
            ViewBag.numberCookie = numberCookie;
            ViewBag.orderTime = now;
            ViewBag.orderId = orderId;


            return View();
        }

        public IActionResult Total()
        {
            string memberIdCookie = Request.Cookies["MemberID"];

            if (memberIdCookie != null)
            {
                
                string memberName = "Matt";
                ViewBag.memberName = memberName;
            }
            else
            {
                return NotFound();
            }
            return View();
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
