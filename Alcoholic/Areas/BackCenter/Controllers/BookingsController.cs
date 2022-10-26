using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alcoholic.Areas.BackCenter.Controllers
{
    [Area("BackCenter")]
    public class BookingsController : Controller
    {
        private readonly db_a8de26_projectContext _db;
        public BookingsController(db_a8de26_projectContext context)
        {
            _db=context;
        }
        public IActionResult Index()
        {
            return View();
        }

    }
}
