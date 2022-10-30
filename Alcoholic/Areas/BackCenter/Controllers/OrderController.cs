using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alcoholic.Areas.BackCenter.Controllers
{
    public class OrderController : Controller
    {
        [Authorize(Roles = "leader,mod,staff")]
        [Area("BackCenter")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
