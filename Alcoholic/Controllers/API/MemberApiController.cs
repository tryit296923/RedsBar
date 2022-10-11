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

        // 更新會員資料
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
