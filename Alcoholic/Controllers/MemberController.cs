using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace Alcoholic.Controllers
{
    public class MemberController : Controller
    {
        private readonly db_a8de26_projectContext db;
        private readonly HashService hash;
        public MemberController(db_a8de26_projectContext db, HashService hash)
        {
            this.db = db;
            this.hash = hash;
        }
        public IActionResult Member()
        {
            return View();
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
            var claim = result.Principal.Claims.FirstOrDefault();
            var account = claim.Subject.Claims.Where(c => c.Type.Contains("emailaddress")).Select(c => c.Value).FirstOrDefault();
            var user = db.Members.Where(x => x.MemberAccount == account).FirstOrDefault();
            if (user != null)
            {
                List<Claim> claims = new(){
                    new Claim(ClaimTypes.Name,user.MemberName),
                    new Claim(ClaimTypes.Role,"member")
                };
                ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                HttpContext.Session.SetString("MemberID", user.MemberID.ToString());
                return RedirectToAction("Cart","Order");
            }
            else
            {
                Member gMem = new()
                {
                    MemberAccount = account,
                    MemberPassword = hash.GetHash(claim.Value),
                    MemberLevel = 0,
                    Salt = Guid.NewGuid().ToString("N"),
                    MemberName = claim.Subject.Name,
                    MemberBirth = new DateTime(0001, 1, 1),
                    Phone = claim.Value,
                    Email = "",
                    Qualified = "n"
                };
                gMem.Email = gMem.MemberAccount;
                db.Members.Add(gMem);
                HttpContext.Session.SetString("MemberID", gMem.MemberID.ToString());
                return View("Legal");
            }
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
    }
}
