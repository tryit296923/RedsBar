using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Security.Policy;
using System.Linq;
using Alcoholic.Models.DTO;
using Alcoholic.Services;
using Newtonsoft.Json;
//using Microsoft.AspNetCore.Cors;

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
        public async Task<ActionResult<IEnumerable<Member>>> GetAllMember(Member memberData)
        {
            return await projectContext.Members.ToListAsync();
        }

        public ActionResult FrontPage()
        {
            return View();
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

        // 入座 => 登入頁面 (導向)
        [HttpPut]
        public async Task<IActionResult> StartOrder(DeskInfo deskInfo)
        {
            deskInfo.Occupied = 1;
            deskInfo.StartTime = DateTime.Now.ToString("yyyyMMddHHmm");
            projectContext.Entry(deskInfo).State = EntityState.Modified;
            await projectContext.SaveChangesAsync();
            HttpContext.Response.Cookies.Append("Desk", deskInfo.Desk);
            HttpContext.Response.Cookies.Append("Number", deskInfo.Number);
            return View("LoginRegister");
        }

        // 登入 => 點餐(Order'Order)
        [HttpPost]
        public async Task<IActionResult> Login(Member memberData)
        {
            Member? user = (from member in projectContext.Members
                            where member.MemberAccount == memberData.MemberAccount
                            && member.MemberPassword == memberData.MemberPassword
                            select member).SingleOrDefault();

            if (user == null)
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

                return RedirectToAction();
                // 在想要經驗證後才可讀取的API上加 [Authorize]
                // 未登入也可以使用的API [AllowAnonymous]
            }
        }

        // 選訪客登入 => 點餐(Order'Order)
        public async Task<IActionResult> GuestLogin()
        {
            Member? user = (from member in projectContext.Members
                            where member.MemberAccount == "guestonly123"
                            && member.MemberPassword == "guestonly123"
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

        // : Member/Register => Member/Getmember
        [HttpPost]
        public async Task<ActionResult> Register(Member memberData)
        {
            if (MemberExists(memberData.MemberAccount))
            {
                return NotFound();
            }
            int number = projectContext.Members.Count() + 100;
            memberData.Qualified = "n";
            memberData.MemberID = DateTime.Now.ToString("yyyyMMdd") + number.ToString();
            await projectContext.AddAsync(memberData);
            await projectContext.SaveChangesAsync();
            mailService.SendMail(memberData.Email, "請點擊下方連結", "RedsBar 會員認證信件");
            return Ok(memberData);
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
    }
}
