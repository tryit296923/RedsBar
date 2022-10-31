using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Specialized;

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
            List<HotSales> hotSales = db.OrderDetails.GroupBy(od => od.ProductId).Select(ods => new HotSales
            {
                ProductName = db.Products.Where(p => p.ProductId == ods.Key).Select(od => od.ProductName).FirstOrDefault(),
                Quantity = db.OrderDetails.Where(od => od.ProductId == ods.Key).Select(od => od.Quantity).Sum(),
                ImgPath = db.ProductImage.Where(i => i.ProductId == ods.Key).Select(od => od.Path).FirstOrDefault(),
            }).OrderByDescending(l => l.Quantity).ToList();
            MainPageModel main = new()
            {
                Total = db.Orders.Select(o => o.Total).Sum().GetValueOrDefault(),
                GuestNum = db.Orders.Select(o => o.Number).Sum(),
                MemberNum = db.Members.Count(),
                Rate = db.Feedback.Select(f => f.Average).Sum().GetValueOrDefault(),
                HotSales = hotSales.GetRange(0, 3)
            };
            return Ok(main);
        }
    }
}