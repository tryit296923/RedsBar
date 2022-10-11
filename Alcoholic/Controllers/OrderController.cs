using Alcoholic.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Alcoholic.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Order()
        {
            return RedirectToAction("Cart", "Order");
        }
        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Success(OrderViewModel order)
        {
            var table = 4;
            order.DeskNum = table.ToString();
            var people = 5;
            order.Number = people.ToString();
            var caseNo = 12345678;
            order.CaseNo = caseNo.ToString();
            var now = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            ViewBag.orderDate = now;
            return View(order);
        }

        public IActionResult Total()
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
