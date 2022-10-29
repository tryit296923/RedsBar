using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Alcoholic.Models.Entities;

namespace Alcoholic.Areas.BackCenter.Controllers
{
    [Area("BackCenter")]
    public class ProductsController : Controller
    {
        private readonly db_a8de26_projectContext _context;

        public ProductsController(db_a8de26_projectContext context)
        {
            _context = context;
        }

        // GET: BackCenter/Products
        public async Task<IActionResult> Index()
        {
              return View(await _context.Products.ToListAsync());
        }
    }
}
