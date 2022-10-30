using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Alcoholic.Areas.BackCenter.Controllers
{
    [Area("BackCenter")]
    public class BackController : Controller
    {
        private readonly db_a8de26_projectContext db;
        public BackController(db_a8de26_projectContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetName()
        {
            string nick = HttpContext.Request.Cookies["nick"];
            return Ok(JsonConvert.SerializeObject(nick));
        }
    }
}