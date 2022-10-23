using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razor.Templating.Core;
using System.Security.Policy;

namespace Alcoholic.Controllers
{
    public class MemberController : Controller
    {
        private readonly db_a8de26_projectContext db;
        private readonly MailService mail;
        private readonly HashService hash;
        public MemberController(db_a8de26_projectContext db, MailService mail, HashService hash)
        {
            this.db = db;
            this.mail = mail;
            this.hash = hash;
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

        // Forget.cshtml => /api/Member/Sendpw => NewPwMail.cshtml => /member/ResetPw => ResetPw.cshtml => api/member/SetNewPw => member/NewPwSuccess 
        public IActionResult Forget()
        {
            return View();
        }
        public IActionResult ResetPw()
        {
            MailModel mailModel = new();
            mailModel.Port = $"{Request.Scheme}://{Request.Host}";
            return View(mailModel);
        }
        public IActionResult NewPwSuccess()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("goGoogle")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> goGoogle()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme); // 拿到個人驗證資料
            return RedirectToAction("Index", "Home");
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
