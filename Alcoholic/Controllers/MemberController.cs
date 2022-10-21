using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace Alcoholic.Controllers
{
    public class MemberController : Controller
    {
        private readonly db_a8de26_projectContext db;
        public MemberController(db_a8de26_projectContext db)
        {
            this.db = db;
        }

        public IActionResult AuthorizeP()
        {
            return View("Authorize");
        }
        public IActionResult FrontPage()
        {
            return View();
        }
        public IActionResult LoginRegister()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Authorize()
        {
            string EmailID = HttpContext.Session.GetString("EmailID");
            if (string.IsNullOrEmpty(EmailID))
            {
                return View("AuthFail");
            }
            Member? memberData = (from member in db.Members
                                  where member.EmailID.ToString() == EmailID
                                  select member).FirstOrDefault();
            memberData.Qualified = "y";
            db.Entry(memberData).State = EntityState.Modified;
            db.SaveChanges();
            return View("AuthSuccess");
        }

        [HttpGet]
        public IActionResult RePass(/*string email*/)
        {
            

            return View();
        }

        [HttpPut]
        public async Task<IActionResult> ModifyData(string id, Member memberData)
        {
            if (id != memberData.MemberID.ToString())
            {
                return BadRequest();
            }
            db.Entry(memberData).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return NoContent();
        }

     
    }

}
