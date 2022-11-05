using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        public IActionResult GetName()
        {
            string nick = HttpContext.Session.GetString("nick");

            return Ok(JsonConvert.SerializeObject(nick));
        }

        public IActionResult HomePageData()
        {
            var hotSales = db.OrderDetails.GroupBy(od => od.ProductId).Select(ods => new HotSales
            {
                ProductName = ods.Select(od => od.Product.ProductName).FirstOrDefault(),
                Quantity = ods.Where(od => od.ProductId == ods.Key).Select(od => od.Quantity).Sum(),
                ImgPath = ods.Select(od => od.Product.Images.FirstOrDefault().Path).FirstOrDefault(),
            }).OrderByDescending(l => l.Quantity).AsNoTracking();
            var orders = db.Orders;
            MainPageModel main = new()
            {
                Total = orders.Select(o => o.Total).Sum(),
                GuestNum = orders.Select(o => o.Number).Sum(),
                MemberNum = db.Members.Count(),
                Rate =  Math.Round(orders.Select(o => o.Feedback).Select(f => f.Average).Average().GetValueOrDefault(),1),
                HotSales = hotSales.ToList().GetRange(0, 5)
            };
            return Ok(main);
        }
    }
}