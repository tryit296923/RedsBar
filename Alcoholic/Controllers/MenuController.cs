using Microsoft.AspNetCore.Mvc;

namespace Alcoholic.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Signature()
        {
            return View();
        }
        public IActionResult Recommendation()
        {
            return View();
        }
    }
}
