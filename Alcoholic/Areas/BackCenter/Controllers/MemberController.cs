using Alcoholic.Models;
using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Alcoholic.Areas.BackCenter.Controllers
{
    [Area("BackCenter")]
    public class MemberController : Controller
    {
        private readonly db_a8de26_projectContext db;
        private readonly HashService hash;

        public MemberController(db_a8de26_projectContext db , HashService hash)
        {
            this.db = db;
            this.hash = hash;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LoginPage()
        {
            return View();
        }
        public IActionResult ResendAuthPage()
        {
            return View();
        }

        public List<List<DataCenterModel>> GetAllMember()
        {
            List<List<DataCenterModel>> result = new();
            var member = db.Members.Where(m => m.Qualified == "y").Select(m => new DataCenterModel
            {
                MemberAccount = m.MemberAccount,
                MemberLevel = m.MemberLevel,
                MemberName = m.MemberName,
                MemberBirth = m.MemberBirth,
                Phone = m.Phone,
                Email = m.Email,
                Age = m.Age.GetValueOrDefault(),
                Qualified =m.Qualified
            }).ToList();
            var premember = db.Members.Where(m => m.Qualified == "n").Select(m => new DataCenterModel
            {
                MemberAccount = m.MemberAccount,
                MemberLevel = m.MemberLevel,
                MemberName = m.MemberName,
                MemberBirth = m.MemberBirth,
                Phone = m.Phone,
                Email = m.Email,
                Age = m.Age.GetValueOrDefault(),
                Qualified = m.Qualified
            }).ToList();
            result.Add(member);
            result.Add(premember);
            return result;
        }

        [HttpPost]
        public IActionResult GetMember([FromBody] string acc)
        {
            var emp = db.Members.Select(e => e).Where(e => e.MemberAccount == acc).FirstOrDefault();
            DataCenterModel dataCenterModel = new()
            {
                MemberAccount = emp.MemberAccount,
                MemberLevel = emp.MemberLevel,
                MemberName = emp.MemberName,
                MemberBirth = emp.MemberBirth,
                Phone = emp.Phone,
                Email = emp.Email,
                Age = emp.Age.GetValueOrDefault(),
                Qualified = emp.Qualified
            };
            return Ok(dataCenterModel);
        }

        [HttpPut]
        public IActionResult EditMember([FromBody] DataCenterModel member)
        {
            ReturnModel returnModel = new();
            if (!ModelState.IsValid)
            //{
            //    returnModel.Status = 2;
            //    return Ok(returnModel);
            //}
            if (!db.Members.Any(Member => Member.MemberAccount == member.MemberAccount))
            {
                returnModel.Status = 1;
                return Ok(returnModel);
            }
            Member mem = db.Members.Select(e => e).Where(e => e.MemberAccount == member.MemberAccount).FirstOrDefault();
            DataCenterModel dataCenterModel = new()
            {
                MemberAccount = mem.MemberAccount,
                MemberLevel = mem.MemberLevel,
                MemberName = mem.MemberName,
                MemberBirth = mem.MemberBirth,
                Phone = mem.Phone,
                Email = mem.Email,
                Age = mem.Age.GetValueOrDefault(),
                Qualified = mem.Qualified
            };
            db.Entry(mem).State = EntityState.Modified;
            db.SaveChanges();
            returnModel.Status = 0;
            return Ok(returnModel);
        }

        [HttpPut]
        public IActionResult StaffGone([FromBody] string acc)
        {
            Member emp = db.Members.Select(e => e).Where(e => e.MemberAccount == acc).FirstOrDefault();
            emp.Qualified = "N";
            db.Entry(emp).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(true);
        }
    }
}
