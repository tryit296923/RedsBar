using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Alcoholic.Controllers.API
{
    [Route("api/[action]")]
    [ApiController]
    public class MemberApiController : ControllerBase
    {
        private readonly db_a8de26_projectContext projectContext;
        private readonly MailService mailService;
        public MemberApiController(db_a8de26_projectContext projectContext, MailService mailService)
        {
            this.projectContext = projectContext;
            this.mailService = mailService;
        }

        // "api/MemberApi"
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMember()
        {
            return await projectContext.Members.ToListAsync();
        }

        // 入座 => 登入頁面
        [HttpPut]
        public async Task<IActionResult> StartOrder(DeskInfo deskInfo)
        {
            deskInfo.Occupied = 1;
            deskInfo.StartTime = DateTime.Now.ToString("yyyyMMddHHmm");
            projectContext.Entry(deskInfo).State = EntityState.Modified;
            await projectContext.SaveChangesAsync();
            HttpContext.Response.Cookies.Append("Desk", deskInfo.Desk);
            HttpContext.Response.Cookies.Append("Desk", deskInfo.Number);
            return RedirectToAction("Login", "MemberApi");
        }

        // 登入
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

                HttpContext.Response.Cookies.Append("MemberID",user.MemberID);

                return RedirectToAction();
                // 在想要經驗證後才可讀取的API上加 [Authorize]
                // 未登入也可以使用的API [AllowAnonymous]
            }
        }

        [HttpDelete]
        public string Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return "Logout";
        }

        // POST: api/Register
        [HttpPost]
        public async Task<ActionResult> Register(Member memberData)
        {
            if (MemberExists(memberData.MemberAccount) && memberData.MemberAccount.Length > 7 && memberData.MemberPassword.Length > 7)
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

        [HttpPut]
        public async Task<bool> Authorize(Member memberData)
        {
            memberData.Qualified = "y";
            projectContext.Entry(memberData).State = EntityState.Modified;
            await projectContext.SaveChangesAsync();
            return true;
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

        private bool MemberExists(string Account)
        {
            return projectContext.Members.Any(member => member.MemberAccount == Account);
        }
    }
}
