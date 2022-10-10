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

        [HttpGet]
        public IActionResult LoginRegister()
        {
            return View("LoginRegister");
        }

        // POST: Member/Register => Member/Getmember
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
        public ActionResult FrontPage()
        {
            return View();
        }
        [HttpPost]
        public bool StartOrder([FromBody] DeskInfo deskInfo)
        {

            return true;
        }

        private bool MemberExists(string Account)
        {
            return projectContext.Members.Any(member => member.MemberAccount == Account);
        }
    }
}
