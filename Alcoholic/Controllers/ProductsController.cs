using Alcoholic.Models.Entities;
using Alcoholic.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Alcoholic.Controllers
{
    public class ProductsController : Controller
    {
        private readonly db_a8de26_projectContext projectContext;

        public ProductsController(db_a8de26_projectContext projectContext)
        {
            this.projectContext = projectContext;
        }
        
        public IActionResult Index()
        {
            return View("OrderTemplate");
        }


    }
}
