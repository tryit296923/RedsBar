using Alcoholic.Models;
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
        public IActionResult CreateDiscount([FromForm] DiscountModel model)
        {
            ReturnModel returnModel = new();
            if (!ModelState.IsValid)
            {
                returnModel.Status = 400;
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return Ok(returnModel);
            }
            else if (DiscountNamerExists(model.DiscountName))
            {
                returnModel.Status = 1;
                return Ok(returnModel);
            }
            else if (model.DiscountAmount == 0)
            {
                returnModel.Status = 2;
                return Ok(returnModel);
            }
            else
            {
                returnModel.Status = 0;
                var discount = new Discount();
                discount.DiscountName = model.DiscountName;
                discount.DiscountAmount = model.DiscountAmount;
                _db.Discount.Add(discount);
                _db.SaveChanges();
                return Ok(returnModel);
            }
        }
        [HttpPut]
        public void EditDiscount([FromForm] DiscountModel editData)
        {

            var discountData = (from x in _db.Discount
                                where x.DiscountId == editData.DiscountId
                                select x).FirstOrDefault();
            discountData.DiscountName = editData.DiscountName;
            discountData.DiscountAmount = editData.DiscountAmount;
            _db.Update(discountData);
            _db.SaveChanges();


        }

        [HttpDelete]
        public void DeleteDiscount([FromBody] int deleteData)
        {
            var discountData = (from x in _db.Discount
                                where x.DiscountId == deleteData
                                select x).FirstOrDefault();
            _db.Remove(discountData);
            _db.SaveChanges();
        }

        //資料庫找重復名字
        private bool DiscountNamerExists(string DiscountName)
        {
            return _db.Discount.Any(discount => discount.DiscountName == DiscountName);
        }
    } 
}
