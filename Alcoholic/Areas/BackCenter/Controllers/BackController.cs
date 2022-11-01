using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Serialization;

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
            }).OrderByDescending(l => l.Quantity);
            var orders = db.Orders;
            MainPageModel main = new()
            {
                Total = orders.Select(o => o.Total).Sum().GetValueOrDefault(),
                GuestNum = orders.Select(o => o.Number).Sum(),
                MemberNum = db.Members.Count(),
                Rate = orders.Select(o => o.Feedback.Average).Sum().GetValueOrDefault(),
                HotSales = hotSales.ToList().GetRange(0, 5)
            };
            return Ok(main);
        }

        [HttpPost]
        public IActionResult SelectedData([FromBody] string choice)
        {
            int days = 30;
            switch (choice)
            {
                case "w": days = 7;
                    break;
                case "m": days = 30;
                    break;
                case "s": days = 90;
                    break;
                case "y": days = 365;
                    break;
            }
            List<Order> orders = new();             
            foreach(Order o in db.Orders)
            {
                if((DateTime.Now - o.OrderTime).TotalDays < days)
                {
                    orders.Add(o);
                }
            }
            List<Member> members = new();
            foreach (Member m in db.Members)
            {
                if ((DateTime.Now - m.Join).TotalDays < days)
                {
                    members.Add(m);
                }
            }
            SelectModel model = new()
            {
                STotal = orders.Select(o => o.Total).Sum().GetValueOrDefault(),
                SGuestNum = orders.Select(o => o.Number).Sum(),
                SMemberNum = members.Count(),
                SRate = orders.Where(o => o.Feedback != null).Select(o => o.Feedback.Average).Average().GetValueOrDefault()
            };
            return Ok(model);
        }
    }
}