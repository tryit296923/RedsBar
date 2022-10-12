using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Alcoholic.Models.DTO;
using Alcoholic.Services;
using Razor.Templating.Core;
using System.Security.Cryptography;
using System.Text;

namespace Alcoholic.Controllers
{
    //[EnableCors] 針對特定Controller或是Action進行設定。
    public class MemberController : Controller
    {
        private readonly db_a8de26_projectContext projectContext;
        private readonly MailService mailService;
        public MemberController(db_a8de26_projectContext projectContext, MailService mailService)
        {
            this.projectContext = projectContext;
            this.mailService = mailService;
        }

        // GET: Member/Getmember
        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetAllMember()
        {
            return await projectContext.Members.ToListAsync();
        }

        //[Authorize(Roles ="moderate")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(Member memberData)
        {
            Member? member = await projectContext.Members.FindAsync(memberData);
            if (member == null)
            {
                return NotFound();
            }
            return member;
        }
        public ActionResult AuthorizeP()
        {
            return View("Authorize");
        }
        public ActionResult FrontPage()
        {
            return View();
        }
        public ActionResult LoginRegister()
        {
            return View();
        }

        // 入座 => 登入頁面 (導向)
        [HttpPut]
        public async Task<IActionResult> StartOrder([FromBody]DeskInfo deskInfo)
        {
            deskInfo.Occupied = 1;
            deskInfo.StartTime = DateTime.Now.ToString("yyyyMMddHHmm");
            projectContext.Entry(deskInfo).State = EntityState.Modified;
            await projectContext.SaveChangesAsync();
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
            Member ? user = (from member in projectContext.Members
                             where member.MemberAccount == memberData.MemberAccount
                             select member).SingleOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            String password = GetHash(memberData.MemberPassword.Concat(user.Salt).ToString());
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
                    new Claim(ClaimTypes.Role, "moderate")
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
        // 登出
        [HttpDelete]
        public string Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return "Logout";
        }
        public async Task<IActionResult> GuestLogin()
        {
            Member? user = (from member in projectContext.Members
                            where member.MemberAccount == "guestonly123"
                            && member.MemberPassword == "d87c5439b038afaf5ada19a23322ca2ac4fd37a74df11559e25784c69ae4"
                            select member).SingleOrDefault();
                var claims = new List<Claim>
                {
                    // new Claim(Claim.Role, "Administrator")
                    new Claim(ClaimTypes.Name, user.MemberName),
                    new Claim(ClaimTypes.Role, "moderate")
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                HttpContext.Response.Cookies.Append("MemberID", user.MemberID);
                return RedirectToAction("Order", "Order");
        }

        // 離席
        [HttpPut]
        public async Task<IActionResult> Dismiss(DeskInfo deskInfo)
        {
            deskInfo.Occupied = 0;
            deskInfo.EndTime = DateTime.Now.ToString("yyyyMMddHHmm");
            projectContext.Entry(deskInfo).State = EntityState.Modified;
            await projectContext.SaveChangesAsync();
            return Ok();
        }

        // 註冊
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Member memberData)
        {
            if (MemberExists(memberData.MemberAccount) && memberData.MemberAccount.Length > 7 && memberData.MemberPassword.Length > 7)
            {
                return NotFound();
            }
            int number = projectContext.Members.Count() + 100;
            string salt = Guid.NewGuid().ToString("N");
            memberData.Salt = salt;
            memberData.MemberPassword = GetHash(memberData.MemberPassword.Concat(salt).ToString());
            memberData.Qualified = "n";
            memberData.MemberID = DateTime.Now.ToString("yyyyMMdd") + number.ToString();
            await projectContext.AddAsync(memberData);
            await projectContext.SaveChangesAsync();
            var msg = await RazorTemplateEngine.RenderAsync<Member>("Views/Member/Authorize.cshtml", memberData);
            mailService.SendMail(memberData.Email, msg, "RedsBar 會員認證信件");
            HttpContext.Response.Cookies.Append("EmailID", memberData.EmailID.ToString());
            return Ok(memberData);
        }

        [HttpPut]
        public async Task<IActionResult> Authorize()
        {
            String cookie = Request.Cookies["EmailID"];
            Member? memberData = (from member in projectContext.Members
                                  where member.EmailID.ToString() == cookie
                                  select member).FirstOrDefault();
            memberData.Qualified = "y";
            projectContext.Entry(memberData).State = EntityState.Modified;
            await projectContext.SaveChangesAsync();
            return Ok(cookie);
        }

        // PUT: Update
        [HttpPut]
        public async Task<IActionResult> ModifyData(string id, Member memberData)
        {
            if (id != memberData.MemberID)
            {
                return BadRequest();
            }
            projectContext.Entry(memberData).State = EntityState.Modified;
            await projectContext.SaveChangesAsync();
            return NoContent();
        }

        private bool MemberExists(string Account)
        {
            return projectContext.Members.Any(member => member.MemberAccount == Account);
        }

        public string GetHash(string input)
        {
            SHA256 sHA256 = SHA256.Create();
            byte[] bytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("X"));
            }
            return builder.ToString().ToLower();
        }

    }
}
