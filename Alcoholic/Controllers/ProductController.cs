using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Alcoholic.Controllers
{
    public class ProductController : Controller
    {
        private readonly db_a8de26_projectContext projectContext;

        public ProductController(db_a8de26_projectContext projectContext)
        {
            this.projectContext = projectContext;
        }
        public ActionResult OrderTemplate()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            return await projectContext.Products.ToListAsync();
        }

        [HttpPost]
        public string AddToCart([FromBody]SendProductsModel model)
        {
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddHours(2));
            string jsonStr = JsonConvert.SerializeObject(model);
            HttpContext.Response.Cookies.Append("memberCart", jsonStr, cookieOptions);
            //var memberId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            //if(memberId == null)
            //{
            //    return NotFound();
            //}
            //var member = projectContext.Members.Include("Order").FirstOrDefault(x => x.MemberID == memberId);
            //if( member == null)
            //{
            //    return NotFound();
            //}

            return "HI";
            
        }
    }
}
