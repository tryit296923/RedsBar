using Microsoft.AspNetCore.Mvc;

namespace Alcoholic.Areas.BackCenter.Controllers
{
    public class OrderController : Controller
    {
        [Area("BackCenter")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
