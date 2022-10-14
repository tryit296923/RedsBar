using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Models.ViewModels;
using Alcoholic.Services;
using Microsoft.AspNetCore.Mvc;

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
            string MemberIdCookie = Request.Cookies["MemberID"];

            
            string userName = (from member in projectContext.Members
                            where member.MemberID == MemberIdCookie
                               select member).SingleOrDefault().MemberName;

            ViewBag.userName = userName;
            return View();
        }

        public IActionResult Success(OrderViewModel order)
        {
            //var table = 4;
            //order.DeskNum = table.ToString();
            //var people = 5;
            //order.Number = people.ToString();
            var caseNo = 12345678;
            order.CaseNo = caseNo.ToString();

            string deskCookie = Request.Cookies["Desk"];
            string numberCookie = Request.Cookies["Number"];

            var now = DateTime.Now.ToString("yyyy/MM/dd HH:mm");

            ViewBag.deskCookie = deskCookie;
            ViewBag.numberCookie = numberCookie;
            ViewBag.orderDate = now;
            return View(order);
        }

        public IActionResult Total()
        {
            return View();
        }
        public IActionResult OrderList()
        {
            return View();
        }
        public IActionResult Check()
        {
            var now = DateTime.Now.ToString("yyyy/MM/dd");
            ViewBag.orderDate = now;
            return View();
        }
    }
}
