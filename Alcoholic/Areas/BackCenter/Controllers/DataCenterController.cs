using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alcoholic.Areas.BackCenter.Controllers
{
    [Authorize(Roles = "leader,mod,staff")]
    [Area("BackCenter")]
    public class DataCenterController : Controller
    {
        private readonly db_a8de26_projectContext db;
        public DataCenterController(db_a8de26_projectContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SelectedData([FromBody] string choice)
        {
            int days = 30;
            switch (choice)
            {
                case "m":
                    days = 30;
                    break;
                case "s":
                    days = 90;
                    break;
                case "y":
                    days = 365;
                    break;
            }
            List<Order> orders = new();
            foreach (Order o in db.Orders)
            {
                if ((DateTime.Now - o.OrderTime).TotalDays < days)
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
            var STotal = orders.GroupBy(o => o.OrderDate).Select(o => new DateTotal
            {
                Date = o.Key,
                Total = o.Select(o => o.Total).Sum()
            });
            var SGuestNum = orders.GroupBy(o => o.OrderDate).Select(o => new DateTotal
            {
                Date = o.Key,
                Total = o.Select(o => o.Number).Sum()
            });
            SelectModel model = new()
            {
                STotal = STotal.ToList(),
                SGuestNum = SGuestNum.ToList(),
                SMemberNum = members.Count(),
            };
            return Ok(model);
        }

    }
}
