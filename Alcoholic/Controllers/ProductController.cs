using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace Alcoholic.Controllers
{
    public class ProductController : Controller
    {
        private readonly db_a8de26_projectContext projectContext;

        public ProductController(db_a8de26_projectContext projectContext)
        {
            this.projectContext = projectContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            return await projectContext.Products.ToListAsync();
        }

        [HttpPost]
        public bool AddToCart(Product product)
        {
            return true;
        }
    }
}
