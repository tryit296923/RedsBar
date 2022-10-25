using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Alcoholic.Areas.BackCenter.Controllers
{
    [Area("BackCenter")]
    public class DataCenterController : Controller
    {
        private readonly db_a8de26_projectContext _db;
        public DataCenterController(db_a8de26_projectContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
