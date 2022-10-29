using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.Twitter;
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
        private readonly LvlService lvl;

        public MemberController(db_a8de26_projectContext db, HashService hash, LvlService lvl)
        {
            this.db = db;
            this.hash = hash;
            this.lvl = lvl;
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
        public IActionResult Member()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("goOauth")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        public IActionResult TwitterLogin()
        {
            var properties = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("goOauth")
            };
            return Challenge(properties, TwitterDefaults.AuthenticationScheme);
        }
        public IActionResult MSLogin()
        {
            var properties = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("goOauth")
            };
            return Challenge(properties, MicrosoftAccountDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> goOauth()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);  // 拿到個人驗證資料
            var claim = result.Principal.Claims.FirstOrDefault();
            var Acc = claim.Subject.Claims.Where(c => c.Type.Contains("emailaddress")).Select(c => c.Value).FirstOrDefault();
            var userMG = db.Members.Where(e => e.MemberAccount == Acc).FirstOrDefault();
            var userT = db.Members.Where(e => e.MemberAccount == claim.Subject.Name).FirstOrDefault();
            var user = userMG == null ? userT : userMG;
            // 已有會員資料(曾經登入過) => SignIn & SetSession 
            if (user != null)
            {
                List<Claim> claims = new(){
                    new Claim(ClaimTypes.Name,user.MemberName),
                    new Claim(ClaimTypes.Role,"member")
                };
                ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                HttpContext.Session.SetString("MemberID", user.MemberID.ToString());
                lvl.MemberLvl(user.MemberAccount);
                return RedirectToAction("Orderlist", "Order");
            }


            var gate = claim.OriginalIssuer.ToString();
            Member oauth = new()
            {
                MemberAccount = "",
                MemberPassword = null,
                MemberLevel = 0,
                Salt = Guid.NewGuid().ToString("N"),
                MemberName = "",
                MemberBirth = new DateTime(0001, 1, 1),
                Phone = "09oAuthLog",
                Email = "noregister@redsbar.com",
                Qualified = "n"
            };
            switch (gate)
            {
                case "Google":
                    {
                        oauth.MemberAccount = Acc;
                        oauth.MemberPassword = hash.GetHash(claim.Value);
                        oauth.MemberName = claim.Subject.Name;
                        oauth.Email = oauth.MemberAccount;
                    }
                    break;
                case "Twitter":
                    {
                        oauth.MemberName = claim.Subject.Name;
                        oauth.MemberAccount = claim.Subject.Name;
                        oauth.MemberPassword = hash.GetHash(claim.Value);
                    }
                    break;
                case "Microsoft":
                    {
                        oauth.MemberName = claim.Subject.Name;
                        oauth.MemberAccount = Acc;
                        oauth.MemberPassword = hash.GetHash(claim.Value);
                        oauth.Email = oauth.MemberAccount;
                    }
                    break;
                default:
                    return BadRequest();
                    break;
            }

            // 無會員資料 => 建立一個資料, 給予Session 但無SignIn, 至年齡判斷
            db.Members.Add(oauth);
            db.SaveChanges();
            HttpContext.Session.SetString("MemberID", oauth.MemberID.ToString());
            return View("Legal");
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
