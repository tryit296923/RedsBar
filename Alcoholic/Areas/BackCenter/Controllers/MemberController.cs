using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Alcoholic.Areas.BackCenter.Controllers
{
    public class MemberController : Controller
    {
        private readonly db_a8de26_projectContext db;

        public MemberController(db_a8de26_projectContext db)
        {
            this.db = db;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Member>> GetAllMember()
        {
            return db.Members.ToList();
        }

        [HttpGet]
        public IActionResult GetMember(Member memberData)
        {
            Member? member = db.Members.Find(memberData);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }
    }
}
