using Alcoholic.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Alcoholic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Oops()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public ReturnModel ToCenter([FromBody] string code)
        {
            string LF2NET = "redshandsome";
            ReturnModel model = new();
            string url = "/backcenter/back";
            if(code == LF2NET)
            {
                model.Url = url;
                model.Status = 200;
                model.Result = true;
                return model;
            }
            else
            {
                model.Url = "";
                model.Status = 400;
                model.Result = false;
                return model;
            }
        }
    }
}