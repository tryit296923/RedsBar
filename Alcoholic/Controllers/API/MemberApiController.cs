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
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return Ok(false);
            }
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

            HttpContext.Session.SetString("Number", deskInfo.Number.ToString());
            HttpContext.Session.SetString("Desk", deskInfo.Desk);

            return Ok(true);
        }

        // 註冊
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] MemberModel memberData)
        {
            //ModelState.IsValid
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return Ok(false);
            }
            if (MemberExists(memberData.Account))
            {
                return Ok(false);
            }
            if (EmailExists(memberData.Email))
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
                MailModel mailModel = new() { Port = $"{Request.Scheme}://{Request.Host}"};
                var msg = RazorTemplateEngine.RenderAsync<MailModel>("Views/Member/Authorize.cshtml", mailModel);
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
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return Ok(false);
            }
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
                MemberLvl(user.MemberAccount);
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
                            && member.MemberPassword == "9e609cccefab80f5ef7aa9745e565b3045588e80dc8b2ba66c98af1a8439ba"
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

        [HttpGet]
        public async Task<IActionResult> Remail()
        {
            string? mailId = HttpContext.Session.GetString("EmailID");
            if (mailId == null || string.IsNullOrEmpty(mailId))
            {
                return new EmptyResult();
            }
            Guid EmailID = Guid.Parse(mailId);
            Member? member = (from m in db.Members
                              where m.EmailID == EmailID
                              select m).FirstOrDefault();
            var msg = await RazorTemplateEngine.RenderAsync<Member>("Views/Member/Authorize.cshtml", member);
            mail.SendMail(member.Email, msg, "RedsBar 會員認證信件");
            return new EmptyResult();
        }

        [HttpPost]
        public async Task<IActionResult> Sendpw([FromBody] dynamic post)
        {
            string email = post.ToString();
            Member? member = db.Members.Select(x => x).Where(x => x.Email == email).FirstOrDefault();
            if (member == null)
            {
                return Ok(false);
            }
            HttpContext.Session.SetString("Email", email);
            MailModel mailModel = new();
            mailModel.Port = $"{Request.Scheme}://{Request.Host}/member/ResetPw";
            var msg = await RazorTemplateEngine.RenderAsync<MailModel>("Views/Member/NewPwMail.cshtml", mailModel);
            mail.SendMail(email, msg, "RedsBar 密碼重設信件");
            return Ok(true);
        }

        public IActionResult SetNewPw([FromBody] string pw)
        {
            string? email = HttpContext.Session.GetString("Email");
            Member? member = db.Members.Select(x => x).Where(x => x.Email == email).FirstOrDefault();
            member.MemberPassword = hash.GetHash(string.Concat(pw, member.Salt).ToString());
            db.Entry(member).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(true);
        }

        [Authorize(Roles = "member")]
        [HttpDelete]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(true);
        }
        public ActionResult<IEnumerable<Member>> GetAllMember()
        {
            return db.Members.ToList();
        }
        public IActionResult GetCurrentMember()
        {
            string? ses = HttpContext.Session.GetString("MemberID");
            if (string.IsNullOrEmpty(ses))
            {
                return Ok(false);
            }
            Guid? memberId = Guid.Parse(ses);

            Member? member = db.Members.Select(x => x).Where(x => x.MemberID == memberId).FirstOrDefault();
            List<int> total = new();
            List<Product> Products = new();
            foreach (Order order in member.Orders)
            {
                foreach (OrderDetail od in order.OrderDetails)
                {
                    total.Add(od.Total);
                    Products.Add(od.Product);
                }
            }
            int sum = total.Sum();
            int disId = member.MemberLevel + 1;
            float dis = (from d in db.Discount where d.DiscountId == disId select d.DiscountAmount).FirstOrDefault();
            var products = Products.GroupBy(x => x.ProductId).OrderByDescending(x => x.Count()).Select(x => x.First()).ToList();
            DataPageModel dataPageModel = new()
            {
                account = member.MemberAccount,
                name = member.MemberName,
                birth = member.MemberBirth,
                mail = member.Email,
                phone = member.Phone,
                total = sum,
                discount = dis,
                products = products
            };

            switch (member.MemberLevel)
            {
                case 0: 
                    dataPageModel.min = 0;
                    dataPageModel.max = 5000;
                    break;
                case 1:
                    dataPageModel.min = 5000;
                    dataPageModel.max = 10000;
                    break;
                case 2:
                    dataPageModel.min = 10000;
                    dataPageModel.max = 30000;
                    break;
                case 3:
                    dataPageModel.min = 30000;
                    dataPageModel.max = 70000;
                    break;
                case 4:
                    dataPageModel.min = 70000;
                    dataPageModel.max = 100000;
                    break;
            }
            return Ok(dataPageModel);
        }

        public List<Order> GetOrders()
        {
            string? ses = HttpContext.Session.GetString("MemberID");
            Guid? memberId = Guid.Parse(ses);
            Member? member = db.Members.Select(x => x).Where(x => x.MemberID == memberId).FirstOrDefault();
            List<Order> details = new();
            foreach(Order o in member.Orders)
            {
                details.Add(o);
            }
            return details;
        }

        [HttpPut]
        public IActionResult EditMember([FromBody] MemberModel memberModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest();
            }
            string? ses = HttpContext.Session.GetString("MemberID");
            if (string.IsNullOrEmpty(ses))
            {
                return Ok(false);
            }
            Guid? memberId = Guid.Parse(ses);

            Member? member = db.Members.Select(x => x).Where(x => x.MemberID == memberId).FirstOrDefault();
            member.MemberAccount = memberModel.Account;
            member.MemberName = memberModel.Name;
            member.MemberBirth = memberModel.Birth;
            member.Email = memberModel.Email;
            member.Phone = memberModel.Phone;
            db.Entry(member).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(true);
        }

        private bool MemberExists(string Account)
        {
            return db.Members.Any(member => member.MemberAccount == Account);
        }
        private bool EmailExists(string Email)
        {
            return db.Members.Any(member => member.Email == Email);
        }
        public void MemberLvl(string account)
        {
            Member? member = (from m in db.Members where m.MemberAccount == account select m).FirstOrDefault();
            if (member == null)
            {
                return;
            }
            List<int> total = new();
            foreach(Order order in member.Orders)
            {
                foreach(OrderDetail od in order.OrderDetails)
                {
                    total.Add(od.Total);
                }
            }
            int sum = total.Sum();
            int level = 0;
            switch (sum)
            {
                case > 70000: level = 4; break;
                case > 30000: level = 3; break;
                case > 10000: level = 2; break;
                case > 5000: level = 1; break;
                default: level = 0; break;
            }
            member.MemberLevel = level;
            db.Entry(member).State = EntityState.Modified;
            db.SaveChanges();
        }

    }
}
