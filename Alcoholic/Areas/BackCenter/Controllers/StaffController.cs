using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using Alcoholic.Services;
using Alcoholic.Models.DTO;
using System.Security.Principal;
using Alcoholic.Models;
using Microsoft.EntityFrameworkCore;

namespace Alcoholic.Areas.BackCenter.Controllers
{
    [Area("BackCenter")]
    public class StaffController : Controller
    {
        private readonly db_a8de26_projectContext db;
        private readonly HashService hash;
        public StaffController(db_a8de26_projectContext db, HashService hash)
        {
            this.db = db;
            this.hash = hash;
        }
        public IActionResult Index()
        {
            return View();
        }
        public List<StaffModel> GetAllMember()
        {
            var staffs = db.Employees.Select(emp => new StaffModel
            {
                EmpName = emp.EmpName,
                EmpAccount = emp.EmpAccount,
                NickName = emp.NickName,
                Contact = emp.Contact,
                Role = emp.Role,
                Status = emp.Status,
                Join = emp.Join
            }).ToList();

            return staffs;
        }

        [HttpPut]
        public IActionResult EditMember([FromBody] StaffModel staff)
        {
            ReturnModel returnModel = new();
            if (!ModelState.IsValid)
            {
                returnModel.Status = 2;
                return Ok(returnModel);
            }
            if (!db.Employees.Any(Member => Member.EmpAccount == staff.EmpAccount))
            {
                returnModel.Status = 1;
                return Ok(returnModel);
            }
            Employee emp = db.Employees.Select(e => e).Where(e => e.EmpAccount == staff.EmpAccount).FirstOrDefault();
            emp.EmpName = staff.EmpName;
            emp.EmpPassword = staff.EmpPassword;
            emp.NickName = staff.NickName;
            emp.Contact = staff.Contact;
            emp.Salary = staff.Salary.GetValueOrDefault();
            emp.Role = staff.Role;
            db.Entry(emp).State = EntityState.Modified;
            db.SaveChanges();
            returnModel.Status = 0;
            return Ok(returnModel);
        }


        [HttpPost]
        public IActionResult Login([FromBody] Employee emp)
        {
            Employee? employee = (from em in db.Employees
                                  where emp.EmpAccount == em.EmpAccount
                                  select em).SingleOrDefault();
            if (employee == null)
            {
                return NotFound();
            }
            if (hash.GetHash(emp.EmpPassword) == employee.EmpPassword)
            {
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.Name,emp.EmpName),
                    new Claim(ClaimTypes.Role,emp.Role),
                };
                ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return Ok();
            }
            return NotFound();
        }
        [Authorize(Roles = "moderater,leader,staff")]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        //[Authorize(Roles = "moderater")]
        [HttpPost]
        public IActionResult Register([FromBody] StaffModel staff)
        {
            ReturnModel returnModel = new();
            if (!ModelState.IsValid)
            {
                returnModel.Status = 2;
                return Ok(returnModel);
            }
            if (db.Members.Any(Member => Member.MemberAccount == staff.EmpAccount))
            {
                returnModel.Status = 1;
                return Ok(returnModel);
            }
            Employee emp = new()
            {
                EmpName = staff.EmpName,
                EmpAccount = staff.EmpAccount,
                NickName = staff.NickName,
                Contact = staff.Contact,
                Salary = staff.Salary.GetValueOrDefault(),
                Role = staff.Role,
                Status = 1,
                Join = DateTime.Now
            };
            emp.Salt = Guid.NewGuid().ToString("N");
            emp.EmpPassword = hash.GetHash(string.Concat(staff.EmpPassword, emp.Salt));
            db.Add(emp);
            db.SaveChanges();
            returnModel.Status = 0;
            return Ok(returnModel);
        }

    }
}
