using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Mvc;
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

        public List<Member> GetAllMember()
        {
            return db.Members.ToList();
        }

        public IActionResult AuthByStaff(string EmailID)
        {
            if (string.IsNullOrEmpty(EmailID))
            {
                return Ok(false);
            }
            Member? memberData = (from member in db.Members
                                  where member.EmailID.ToString() == EmailID
                                  select member).FirstOrDefault();
            if (memberData == null)
            {
                return Ok(false);
            }
            memberData.Qualified = "y";
            db.Entry(memberData).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(true);
        }
    }
}
