using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alcoholic.Controllers.API
{
    [Route("api/Discount/[action]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly db_a8de26_projectContext _db;

        public DiscountController(db_a8de26_projectContext db)
        {
            this._db = db;
        }
        [HttpGet]
        public IEnumerable<DiscountModel> GetDiscount()
        {
            var disLv = from lv in _db.Discount
                        select new DiscountModel
                        {
                            DiscountId = lv.DiscountId,
                            DiscountName = lv.DiscountName,
                            DiscountAmount = lv.DiscountAmount,
                        };

            return disLv;
        }

        [HttpPost]
        public void CreateDiscount([FromForm] DiscountModel model)
        {
            var discount = new Discount()
            {
                DiscountName = model.DiscountName,
                DiscountAmount = model.DiscountAmount,
            };
            _db.Discount.Add(discount);
            _db.SaveChanges();
        }
        [HttpPost]
        public IActionResult EditDiscount([FromForm] DiscountModel editData)
        {
            var discountData = (from x in _db.Discount
                               where x.DiscountId == editData.DiscountId
                                select x).FirstOrDefault();
            
            if (editData.DiscountId != 0)
            {
                discountData.DiscountId = editData.DiscountId;
                discountData.DiscountName = editData.DiscountName;
                discountData.DiscountAmount = editData.DiscountAmount;
                _db.Update(discountData);
                _db.SaveChanges();
            }

            
            return Ok(true);
        }
    }
}
