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
//using Microsoft.AspNetCore.Cors;

namespace Alcoholic.Controllers
{
    //[EnableCors] 針對特定Controller或是Action進行設定。
    public class MemberController : Controller
    {
        private readonly db_a8de26_projectContext _projectContext;
        private readonly MailService _mailService;
        public MemberController(db_a8de26_projectContext projectContext, MailService mailService)
        {
            _projectContext = projectContext;
            _mailService = mailService;
        }

        // GET: Member/Getmember
        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetAllMember(Member memberData)
        {
            return await _projectContext.Members.ToListAsync();
        }
        //[Authorize(Roles ="moderate")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(Member memberData)
        {
            Member? member = await _projectContext.Members.FindAsync(memberData);
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
        public async Task<bool> Register(Member memberData)
        {
            if (MemberExists(memberData.MemberAccount))
            {
                return false;
            }
            int number = _projectContext.Members.Count() + 100;
            memberData.Qualified = "n";
            memberData.MemberID = DateTime.Now.ToString("yyyyMMdd") + number.ToString();
            await _projectContext.AddAsync(memberData);
            await _projectContext.SaveChangesAsync();
            _mailService.SendMail(memberData.Email, "請點擊下方連結", "RedsBar 會員認證信件");
            return true;
        }

        // PUT: Update
        [HttpPut]
        public async Task<IActionResult> ModifyData(string id, Member memberData)
        {
            if (id != memberData.MemberID)
            {
                return BadRequest();
            }
            _projectContext.Entry(memberData).State = EntityState.Modified;
            await _projectContext.SaveChangesAsync();
            return NoContent();
        }
        public ActionResult FrontPage()
        {
            return View();
        }
        [HttpPost]
        public bool StartOrder([FromBody] DeskInfo deskInfo)
        {
            string desk = deskInfo.Desk;
            string number = deskInfo.Number;
            return true;
        }

        private bool MemberExists(string Account)
        {
            return _projectContext.Members.Any(member => member.MemberAccount == Account);
        }
    }
}
