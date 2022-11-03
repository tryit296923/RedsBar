using Alcoholic.Models;
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
                       //不顯示客製化商品
                       where pro.ProductId != 52
                       select new MenuModel
                       {
                           Id = pro.ProductId,
                           Name = pro.ProductName,
                           Description = pro.ProductDescription,
                           Price = pro.UnitPrice,
                           Path = path.Path,
                           DiscountId= pro.DiscountId,
                           DiscountAmount=pro.Discount.DiscountAmount
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
        [HttpGet]
        public IEnumerable<MenuModel> GetRecommendHeadProd()
        {
            var recHeadProd = from pro in _db.Products
                       join dis in _db.Discount
                       on pro.DiscountId equals dis.DiscountId
                       join path in _db.ProductImage
                       on pro.ProductId equals path.ProductId
                        //熱銷商品的折扣ID
                        where dis.DiscountId == 5
                       select new MenuModel
                       {
                           Id = pro.ProductId,
                           Name = pro.ProductName,
                           Price = pro.UnitPrice,
                           Description = pro.ProductDescription,
                           DiscountId = dis.DiscountId,
                           DiscountName = dis.DiscountName,
                           Path = path.Path
                       };

            return recHeadProd;
        }
        [HttpGet]
        public IEnumerable<MenuModel> GetRecommendBodyProd()
        {
            var recHeadProd = from pro in _db.Products
                              join dis in _db.Discount
                              on pro.DiscountId equals dis.DiscountId
                              join path in _db.ProductImage
                              on pro.ProductId equals path.ProductId
                              //熱銷商品的折扣ID
                              where dis.DiscountId == 13 || dis.DiscountId == 14
                              select new MenuModel
                              {
                                  Id = pro.ProductId,
                                  Name = pro.ProductName,
                                  Price = pro.UnitPrice,
                                  Description = pro.ProductDescription,
                                  DiscountId = dis.DiscountId,
                                  DiscountName = dis.DiscountName,
                                  Path = path.Path
                              };

            return recHeadProd;
        }
        [HttpGet]
        public IEnumerable<Materials> GetMaterialCategory()
        {
            var mats = from x in _db.Category
                       join y in _db.Materials
                       on x.CategoryId equals y.CategoryId

                       select new Materials
                       {
                           Id = x.CategoryId,
                           MaterialName = y.Category.CategoryName + "-" + y.Name,
                           MaterialId = y.MaterialId,
                           Consumption = 0
                       };
            return mats.OrderBy(x=>x.MaterialName);
        }
        
        [HttpPost]
        public IActionResult CreateProduct([FromForm]CreateProductModel model)
        {

            ReturnModel returnModel = new();
            if (!ModelState.IsValid)
            {
                returnModel.Status = 400;
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return Ok(returnModel);
            }
            else if (model.Cost <= 0 || model.UnitPrice<=0)
            {
                returnModel.Status = 1;
                return Ok(returnModel);
            }
            else
            {
                returnModel.Status = 0;
                const string prefix = @"\images\Products\";
                var folder = env.WebRootPath + prefix;
                var tempFilePath = new List<string>();
                if (model.Images != null)
                {
                    var index = 0;
                    foreach (var img in model.Images)
                    {
                        if (img.Length > 0)
                        {
                            var fileName = $@"{DateTime.Now.Ticks}-{index}-{img.FileName}";
                            using (var sm = new FileStream($@"{folder}\{fileName}", FileMode.Create))
                            {
                                img.CopyTo(sm);
                                tempFilePath.Add(prefix + fileName);
                                index++;
                            }
                        }

                    }
                }
                //新增商品至Product資料表
                var prod = new Product()
                {
                    Cost = model.Cost,
                    UnitPrice = model.UnitPrice,
                    ProductName = model.ProductName,
                    ProductDescription = model.ProductDescription,
                    DiscountId = model.DiscountId,
                    Images = tempFilePath.Select(x => new ProductImage() { Path = x }).ToList(),
                    ProductsMaterials= model.Materials.Select(x=> {
                        var temp = JsonConvert.DeserializeObject<Materials>(x);
                        return new ProductsMaterials
                        {
                            MaterialId = temp.MaterialId,
                            Consumption = temp.Consumption
                        };
                    }).ToList(),
                };
                _db.Products.Add(prod);
                _db.SaveChanges();
                
                return Ok(returnModel);
            }
            
        }
        [HttpPut]
        public void EditProduct([FromForm] EditProductModel editData)
        {
            var productData= (from x in _db.Products 
                             where x.ProductId==editData.ProductId
                             select x).FirstOrDefault();
                productData.ProductName = editData.ProductName;
                productData.ProductDescription = editData.ProductDescription;
                productData.Cost = editData.Cost;
                productData.UnitPrice = editData.UnitPrice;
             productData.DiscountId = editData.DiscountId;

            
            _db.Update(productData);
            _db.SaveChanges();
        }
        [HttpDelete]
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
