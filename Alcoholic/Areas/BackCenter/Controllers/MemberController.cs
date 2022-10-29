using Alcoholic.Models;
using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

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
        [Authorize(Roles = "leader")]
        public IActionResult Index()
        {
            return View();
        }



        [Authorize(Roles = "leader,mod,staff")]
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
                Qualified = m.Qualified
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
        [Authorize(Roles = "leader,mod,staff")]
        [HttpPost]
        public IActionResult GetMember([FromBody] string acc)
        {
            var emp = db.Members.Select(e => e).Where(e => e.MemberAccount == acc).FirstOrDefault();
            EditModel member = new()
            {
                MemberAccount = emp.MemberAccount,
                MemberLevel = emp.MemberLevel,
                MemberName = emp.MemberName,
                Phone = emp.Phone,
                Email = emp.Email,
                Qualified = emp.Qualified
            };
            return Ok(member);
        }
        [Authorize(Roles = "leader,mod")]
        [HttpPut]
        public IActionResult EditMember([FromBody] EditModel member)
        {
            ReturnModel returnModel = new();
            if (!ModelState.IsValid)
            {
                returnModel.Status = 2;
                return Ok(returnModel);
            }
            if (!db.Members.Any(Member => Member.MemberAccount == member.MemberAccount))
            {
                returnModel.Status = 1;
                return Ok(returnModel);
            }
            Member mem = db.Members.Select(e => e).Where(e => e.MemberAccount == member.MemberAccount).FirstOrDefault();
            mem.MemberAccount = member.MemberAccount;
            mem.MemberLevel = member.MemberLevel;
            mem.MemberName = member.MemberName;
            mem.Phone = member.Phone;
            mem.Email = member.Email;
            mem.Qualified = member.Qualified;

            db.Entry(mem).State = EntityState.Modified;
            db.SaveChanges();
            returnModel.Status = 0;
            return Ok(returnModel);
        }
        [Authorize(Roles = "leader")]
        [HttpPut]
        public IActionResult MemberGone([FromBody] string acc)
        {
            Member mem = db.Members.Select(e => e).Where(e => e.MemberAccount == acc).FirstOrDefault();
            mem.Qualified = "n";
            db.Entry(mem).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(true);
        }
        [Authorize(Roles = "leader")]
        [HttpPut]
        public IActionResult MemberRemove([FromBody] string acc)
        {
            Member mem = db.Members.Select(e => e).Where(e => e.MemberAccount == acc).FirstOrDefault();
            mem.MemberAccount = "deletedAcc";
            mem.Qualified = "X";
            mem.MemberLevel = 0;
            mem.MemberName = "XXX";
            mem.Phone = "notamember";
            mem.Email = "xxx@xxx.com";
            db.Entry(mem).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(true);
        }
    }
}
