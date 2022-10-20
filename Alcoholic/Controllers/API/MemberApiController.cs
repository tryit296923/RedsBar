using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Razor.Templating.Core;
using Microsoft.AspNetCore.Http;

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

            HttpContext.Session.SetString("Number", deskInfo.Number);
            HttpContext.Session.SetString("Desk", deskInfo.Desk.ToString());

            return Ok();
        }

        // 註冊
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] MemberModel memberData)
        {
            if (MemberExists(memberData.Account))
            {
                return Ok(false);
            }
            else
            {
                Member user = new();
                string salt = Guid.NewGuid().ToString("N");
                user.MemberAccount = memberData.Account;
                user.MemberBirth = memberData.Birth;
                user.MemberName = memberData.Name;
                user.Phone = memberData.Phone;
                user.Email = memberData.Email;
                user.MemberLevel = 0;
                user.Salt = salt;
                user.Qualified = "n";
                user.MemberPassword = hash.GetHash(string.Concat(memberData.Password, salt).ToString());
                var msg = RazorTemplateEngine.RenderAsync<Member>("Views/Member/Authorize.cshtml", user);
                db.Add(user);
                db.SaveChanges();
                var result = await msg;
                mail.SendMail(memberData.Email, result, "RedsBar 會員認證信件");

                HttpContext.Session.SetString("EmailID", user.EmailID.ToString());
                return Ok(true);
            }
        }

        // 會員登入 => 點餐(Order'Order)
        [HttpPost]
        public IActionResult Login([FromBody] LoginViewModel memberData)
        {
            Member? user = (from member in db.Members
                            where member.MemberAccount == memberData.Account
                            select member).SingleOrDefault();
            if (user == null)
            {
                return Ok(false);
            }
            string password = hash.GetHash(string.Concat(memberData.Password, user.Salt).ToString());
            if (password != user.MemberPassword)
            {
                return Ok(false);
            }
            else
            {
                // 驗證
                var claims = new List<Claim>
                {
                    // new Claim(Claim.Role, "Administrator")
                    new Claim(ClaimTypes.Name, user.MemberName),
                    new Claim(ClaimTypes.Role, "member")
                };
                // 將 Claim 設定引入 ClaimsIdentity類別
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                // SignInAsync(scheme,principal)
                // 將此類別(原則 ClaimsIdentity)，帶入方案(AuthenticationScheme)中
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                HttpContext.Session.SetString("MemberID", user.MemberID.ToString());
                return Ok(true);
                // 在想要經驗證後才可讀取的API上加 [Authorize]
                // 未登入也可以使用的API [AllowAnonymous]
            }
        }


        // 訪客登入 => 點餐(Order'Order)
        public async Task<IActionResult> GuestLogin()
        {
            Member? user = (from member in db.Members
                            where member.MemberAccount == "guestonly123"
                            && member.MemberPassword == "d87c5439b038afaf5ada19a23322ca2ac4fd37a74df11559e25784c69ae4"
                            select member).SingleOrDefault();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.MemberName),
                new Claim(ClaimTypes.Role, "Guest")
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            HttpContext.Session.SetString("MemberID", user.MemberID.ToString());
            return RedirectToAction("Cart", "Order");
        }

        [Authorize(Roles = "member")]
        [HttpDelete]
        public string Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return "Logout";
        }

        [HttpPut]
        public async Task<IActionResult> Reset(string MemberAccount)
        {
            if (string.IsNullOrEmpty(MemberAccount))
            {
                return NotFound();
            }
            Member? member = (from mem in db.Members
                              where MemberAccount == mem.MemberAccount
                              select mem).FirstOrDefault();
            if (member == null)
            {
                return NotFound();
            }
            var msg = await RazorTemplateEngine.RenderAsync<Member>("Views/Member/Authorize.cshtml", member);
            mail.SendMail(member.Email, msg, "密碼重設信件");
            return Ok();
        }

        private bool MemberExists(string Account)
        {
            return db.Members.Any(member => member.MemberAccount == Account);
        }

    }
}
