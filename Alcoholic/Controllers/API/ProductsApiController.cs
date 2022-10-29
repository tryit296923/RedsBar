using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public IEnumerable<BackProdModel> GetAllBackProducts()
        {
            var prod = from pro in _db.Products
                       join path in _db.ProductImage
                       on pro.ProductId equals path.ProductId
                       select new BackProdModel
                       {
                           Id = pro.ProductId,
                           Name = pro.ProductName,
                           Description = pro.ProductDescription,
                           Price = pro.UnitPrice,
                           Path = path.Path,
                           Cost = pro.Cost,
                           DiscountId = pro.Discount.DiscountId,
                           DiscountName = pro.Discount.DiscountName,
                       };

            return prod;
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
                ProductDescription = model.ProductDescription,
                Images = tempFilePath.Select(x=> new ProductImage() { Path = x}).ToList()
            };
            _db.Products.Add(prod);
            _db.SaveChanges();
        }
        [HttpPost]
        public void EditProduct([FromForm] EditProductModel editData)
        {
            var productData= (from x in _db.Products 
                             where x.ProductId==editData.ProductId
                             select x).FirstOrDefault();
                productData.ProductName = editData.ProductName;
                productData.ProductDescription = editData.ProductDescription;
                productData.Cost = editData.Cost;
                productData.UnitPrice = editData.UnitPrice;

            
            _db.Update(productData);
            _db.SaveChanges();
        }
        [HttpPost]
        public void DeleteProduct([FromBody]int productId)
        {
            var productDelete = (from x in _db.Products
                                 where x.ProductId == productId
                                 select x).FirstOrDefault();
            _db.Remove(productDelete);
            _db.SaveChanges();
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
