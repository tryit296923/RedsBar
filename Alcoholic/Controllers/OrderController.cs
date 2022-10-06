using Microsoft.AspNetCore.Mvc;

namespace Alcoholic.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Total()
        {
            return View();
        }
        public IActionResult Check()
        {
            return View();
        }
    }
}
