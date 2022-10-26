using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Mvc;

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

    }
}
