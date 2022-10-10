using Alcoholic.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Alcoholic.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Success(OrderViewModel order)
        {
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
            return View();
        }

        public IActionResult Payment()
        {
            return View();
        }
    }
}
