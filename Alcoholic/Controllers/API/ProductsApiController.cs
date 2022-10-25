using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Linq;
using System.Xml.Linq;

namespace Alcoholic.Controllers.API
{
    [Route("api/Products/[action]")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly db_a8de26_projectContext _db;
        private readonly IWebHostEnvironment env;

        public ProductsApiController(db_a8de26_projectContext projectContext,IWebHostEnvironment env)
        {
            this._db = projectContext;
            this.env = env;
        }
        [HttpPost]
        public void CreateProduct([FromForm]CreateProductModel model)
        {
            const string prefix = @"\images\Products\";
            var folder = env.WebRootPath+prefix;
            var tempFilePath = new List<string>();
            if (model.Images != null) {
                var index = 0;
                foreach (var img in model.Images)
                {
                    if (img.Length > 0)
                    {
                        var fileName = $@"{DateTime.Now.Ticks}-{index}-{img.FileName}";
                        using (var sm = new FileStream($@"{folder}\{fileName}", FileMode.Create)) 
                        {
                            img.CopyTo(sm);
                            tempFilePath.Add(prefix+fileName);
                            index++;
                        }
                    }

                }
            }
            var prod = new Product() {
                Cost = model.Cost,
                UnitPrice = model.UnitPrice,
                ProductName = model.ProductName,
                ProductDescription = "",
                Images = tempFilePath.Select(x=> new ProductImage() { Path = x}).ToList()
            };
            _db.Products.Add(prod);
            _db.SaveChanges();
        }

        [HttpGet]
        public IEnumerable<MenuModel> GetAllProducts()
        {
            var menu = from pro in _db.Products
                    join path in _db.ProductImage
                    on pro.ProductId equals path.ProductId
                    select new MenuModel
                    {
                        Id = pro.ProductId,
                        Name = pro.ProductName,
                        Description = pro.ProductDescription,
                        Price = pro.UnitPrice,
                        Path = path.Path
                    };

            return menu;
        }
        [HttpGet]
        public IEnumerable<BackProd> GetAllBackProducts()
        {
            var prod = from pro in _db.Products
                       join path in _db.ProductImage
                       on pro.ProductId equals path.ProductId
                       select new BackProd
                       {
                           Id = pro.ProductId,
                           Name = pro.ProductName,
                           Description = pro.ProductDescription,
                           Price = pro.UnitPrice,
                           Path = path.Path,
                           Cost = pro.Cost,
                       };

            return prod;
        }

        [HttpPost]
        public string AddToCart([FromBody] SendProductsModel model)
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
            //var member = _db.Members.Include("Order").FirstOrDefault(x => x.MemberID == memberId);
            //if( member == null)
            //{
            //    return NotFound();
            //}

            return "HI";

        }
    }

}
