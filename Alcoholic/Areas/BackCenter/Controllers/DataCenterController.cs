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

        [HttpPost]
        public IActionResult SelectedData([FromBody] int days)
        {
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
            List<OrderDetail> ods = new();
            foreach (OrderDetail od in db.OrderDetails)
            {
                if ((DateTime.Now - od.Order.OrderTime).TotalDays < days)
                {
                    ods.Add(od);
                }
            }
            List<DateTotal> STotal = new();
            List<ProductTotal> ProductTotal = new();
            switch (days)
            {
                case 30:
                    {
                        STotal = orders.OrderByDescending(o => o.OrderTime).GroupBy(o => o.OrderTime.Day).Select(o => new DateTotal
                        {
                            Date = o.Key,
                            Total = o.Select(o => o.Total).Sum(),
                            GuestTotal = o.Select(o => o.Number).Sum(),
                        }).ToList();
                        ProductTotal = ods.GroupBy(od => od.Product.ProductName).Select(od => new ProductTotal
                        {
                            ProductName = od.Key,
                            Quantity = od.Select(o => o.Quantity).Sum(),
                        }).OrderByDescending(pt => pt.Quantity).ToList();
                    }
                    break;
                case 90:
                    {
                        STotal = orders.OrderByDescending(o => o.OrderTime).GroupBy(o => o.OrderTime.Day).Select(o => new DateTotal
                        {
                            Date = o.Key,
                            Total = o.Select(o => o.Total).Sum(),
                            GuestTotal = o.Select(o => o.Number).Sum()
                        }).ToList();
                        ProductTotal = ods.GroupBy(od => od.Product.ProductName).Select(od => new ProductTotal
                        {
                            ProductName = od.Key,
                            Quantity = od.Select(o => o.Quantity).Sum(),
                        }).OrderByDescending(pt => pt.Quantity).ToList();
                    }
                    break;
                case 365:
                    {
                        STotal = orders.OrderByDescending(o => o.OrderTime).GroupBy(o => o.OrderTime.Month).Select(o => new DateTotal
                        {
                            Date = o.Key,
                            Total = o.Select(o => o.Total).Sum(),
                            GuestTotal = o.Select(o => o.Number).Sum()
                        }).ToList();
                        ProductTotal = ods.GroupBy(od => od.Product.ProductName).Select(od => new ProductTotal
                        {
                            ProductName = od.Key,
                            Quantity = od.Select(o => o.Quantity).Sum(),
                        }).OrderByDescending(pt => pt.Quantity).ToList();
                    }
                    break;
            }
            SelectModel model = new()
            {
                ProductTotal = ProductTotal,
                STotal = STotal,
                SMemberNum = members.Count(),
                Orders = orders.Count(),
                Avg = orders.Select(o => o.Total).Average()
            };
            return Ok(model);
        }

        public IActionResult GetMaterials()
        {
            List<MaterialsInv> materialsInvs = db.Materials.Select(m => new MaterialsInv
            {
                Name = m.Name,
                Inventory = m.Inventory,
                CategoryName = m.Category.CategoryName,
                Id = m.MaterialId
            }).ToList();
            return Ok(materialsInvs);
        }
    }
}
