//using Alcoholic.Models.DTO;
//using Alcoholic.Models.Entities;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Drawing.Text;
//using System.Security.Claims;
//using Microsoft.AspNetCore.Authorization;
//using System.Security.Policy;
//using System.Linq;
////using Microsoft.AspNetCore.Cors;

//namespace Alcoholic.Controllers
//{
//    //[EnableCors] 針對特定Controller或是Action進行設定。
//    public class MemberController : Controller
//    {
//        private ProjectContext _projectContext;
//        public MemberController(ProjectContext projectContext)
//        {
//            _projectContext = projectContext;
//        }

//        // GET: Member/Getmember
//        //[Authorize]
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<MembersModel>>> GetAllMember(MembersModel memberData)
//        {
//            return await _projectContext.Members.ToListAsync();
//        }
//        //[Authorize(Roles ="moderate")]
//        [HttpGet("{id}")]
//        public async Task<ActionResult<MembersModel>> GetMember(MembersModel memberData)
//        {
//            MembersModel? member = await _projectContext.Members.FindAsync(memberData);
//            if (member == null)
//            {
//                return NotFound();
//            }
//            return member;
//        }

//        [HttpGet]
//        public IActionResult LoginRegister()
//        {
//            return View("LoginRegister");
//        }

//        // POST: Member/Register => Member/Getmember
//        [HttpPost]
//        public async Task<bool> Register(MembersModel memberData)
//        {
//            if (MemberExists(memberData.MemberAccount))
//            {
//                return false;
//            }
//            int number = _projectContext.Members.Count() + 100;
//            memberData.MemberID = DateTime.Now.ToString("yyyyMMdd") + number.ToString();
//            await _projectContext.AddAsync(memberData);
//            await _projectContext.SaveChangesAsync();

//            return true;
//        }

//        // PUT: Update
//        [HttpPut]
//        public async Task<IActionResult> ModifyData(string id, MembersModel memberData)
//        {
//            if(id != memberData.MemberID)
//            {
//                return BadRequest();
//            }
//            _projectContext.Entry(memberData).State = EntityState.Modified;
//            await _projectContext.SaveChangesAsync();
//            return NoContent();
//        }

//        private bool MemberExists(string Account)
//        {
//            return _projectContext.Members.Any(member => member.MemberAccount == Account);
//        }
//    }
//}
