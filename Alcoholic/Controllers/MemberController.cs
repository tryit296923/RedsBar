using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Alcoholic.Models.DTO;
using Alcoholic.Services;
using Razor.Templating.Core;
using Microsoft.AspNetCore.Authorization;

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

        // 入座 => 登入頁面 (導向)
        [HttpPut]
        public IActionResult StartOrder([FromBody]DeskInfo deskInfo)
        {
            deskInfo.Occupied = 1;
            deskInfo.StartTime = DateTime.Now.ToString("yyyyMMddHHmm");
            db.Entry(deskInfo).State = EntityState.Modified;
            db.SaveChanges();
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddHours(2));
            HttpContext.Response.Cookies.Append("Number", deskInfo.Number, cookieOptions);
            HttpContext.Response.Cookies.Append("Desk", deskInfo.Desk, cookieOptions);
            return Ok("LoginRegister");
        }
        // 會員登入 => 點餐(Order'Order)
        [HttpPost]
        public IActionResult Login([FromBody] Member memberData)
        {
            Member? user = (from member in db.Members
                             where member.MemberAccount == memberData.MemberAccount
                             select member).SingleOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            String password = hash.GetHash(memberData.MemberPassword.Concat(user.Salt).ToString());
            if (password != user.MemberPassword)
            {
                return NotFound();
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
                HttpContext.Response.Cookies.Append("MemberID", user.MemberID);
                return RedirectToAction("Order", "Order");
                // 在想要經驗證後才可讀取的API上加 [Authorize]
                // 未登入也可以使用的API [AllowAnonymous]
            }
        }

        [Authorize(Roles = "member")]
        [HttpDelete]
        public string Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return "Logout";
        }

        // 訪客登入 => 點餐(Order'Order)
        [HttpPost]
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
            HttpContext.Response.Cookies.Append("MemberID", user.MemberID);
            return RedirectToAction("OrderTemplate","Product");
        }

        // 離席
        [HttpPut]
        public IActionResult Dismiss(DeskInfo deskInfo)
        {
            deskInfo.Occupied = 0;
            deskInfo.EndTime = DateTime.Now.ToString("yyyyMMddHHmm");
            db.Entry(deskInfo).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        // 註冊
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Member memberData)
        {
            if (MemberExists(memberData.MemberAccount))
            {
                return NotFound();
            }
            else
            {
                string salt = Guid.NewGuid().ToString("N");
                memberData.Salt = salt;
                memberData.MemberID = Guid.NewGuid().ToString("N");
                memberData.MemberPassword = hash.GetHash(memberData.MemberPassword.Concat(salt).ToString());
                memberData.Qualified = "n";
                db.Add(memberData);
                db.SaveChanges();
                var msg = await RazorTemplateEngine.RenderAsync<Member>("Views/Member/Authorize.cshtml", memberData);
                mail.SendMail(memberData.Email, msg, "RedsBar 會員認證信件");
                HttpContext.Response.Cookies.Append("EmailID", memberData.EmailID.ToString());
                return Ok(memberData);
            }
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

        [HttpPut]
        public async Task<IActionResult> ModifyData(string id, Member memberData)
        {
            if (id != memberData.MemberID)
            {
                return BadRequest();
            }
            db.Entry(memberData).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return NoContent();
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
            mail.SendMail(member.Email,msg,"密碼重設信件");
            return Ok();
        }

        private bool MemberExists(string Account)
        {
            return db.Members.Any(member => member.MemberAccount == Account);
        }
    }
}
