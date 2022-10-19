using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alcoholic.Controllers.API
{
    [Route("api/member/[action]")]
    [ApiController]
    public class MemberApiController : ControllerBase
    {
        private readonly db_a8de26_projectContext db;
        private readonly MailService mail;
        private readonly HashService hash;
        public MemberApiController(db_a8de26_projectContext db, MailService mail, HashService hash)
        {
            this.db = db;
            this.mail = mail;
            this.hash = hash;
        }
        // 入座 => 登入頁面 (導向)
        [HttpPut]
        public IActionResult StartOrder([FromBody] DeskModel desk)
        {
            DeskInfo? deskInfo = (from d in db.DeskInfo
                                  where d.Desk == desk.Desk
                                  select d).FirstOrDefault();
            if(deskInfo == null)
            {
                return NotFound();
            }
            deskInfo.Occupied = 1;
            deskInfo.StartTime = DateTime.Now.ToString("yyyyMMddHHmm");
            db.Entry(deskInfo).State = EntityState.Modified;
            db.SaveChanges();
            CookieOptions cookieOptions = new();
            cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddHours(2));
            HttpContext.Response.Cookies.Append("Number", deskInfo.Number, cookieOptions);
            HttpContext.Response.Cookies.Append("Desk", deskInfo.Desk.ToString(), cookieOptions);

            return Ok();
        }

        [HttpPut]
        public IActionResult Authorize()
        {
            string cookie = Request.Cookies["EmailID"];
            Member? memberData = (from member in db.Members
                                  where member.EmailID.ToString() == cookie
                                  select member).FirstOrDefault();
            memberData.Qualified = "y";
            db.Entry(memberData).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(cookie);
        }
    }
}
