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
        [Authorize(Roles = "leader,mod,staff")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "leader,mod,staff")]
        public IActionResult GetAllMember()
        {

            var staffs = db.Employees.Where(s => s.Status == 1).Select(emp => new StaffModel
            {
                EmpName = emp.EmpName,
                EmpAccount = emp.EmpAccount,
                NickName = emp.NickName,
                Contact = emp.Contact,
                Role = emp.Role,
                Status = emp.Status,
                Join = emp.Join,
            }).ToList();
            var gstaffs = db.Employees.Where(s => s.Status == 0).Select(emp => new StaffModel
            {
                EmpName = emp.EmpName,
                EmpAccount = emp.EmpAccount,
                NickName = emp.NickName,
                Contact = emp.Contact,
                Role = emp.Role,
                Status = emp.Status,
                Join = emp.Join,
                Leave = emp.Leave,
            }).ToList();
            List<List<StaffModel>> lists = new();
            lists.Add(staffs);
            lists.Add(gstaffs);
            return Ok(lists);
        }
        [Authorize(Roles = "leader,mod,staff")]
        [HttpPost]
        public IActionResult GetMember([FromBody] string acc)
        {
            var emp = db.Employees.Select(e => e).Where(e => e.EmpAccount == acc).FirstOrDefault();
            StaffModel staffEditModel = new()
            {
                EmpName = emp.EmpName,
                EmpAccount = emp.EmpAccount,
                NickName = emp.NickName,
                Contact = emp.Contact,
                Role = emp.Role,
                Join = emp.Join
            };
            return Ok(staffEditModel);
        }
        [Authorize(Roles = "leader,mod")]
        [HttpPut]
        public IActionResult EditMember([FromBody] StaffModel model)
        {
            ReturnModel returnModel = new();
            if (!ModelState.IsValid)
            {
                returnModel.Status = 2;
                return Ok(returnModel);
            }
            if (!db.Employees.Any(e => e.EmpAccount == model.EmpAccount))
            {
                returnModel.Status = 1;
                return Ok(returnModel);
            }
            Employee emp = db.Employees.Select(e => e).Where(e => e.EmpAccount == model.EmpAccount).FirstOrDefault();
            emp.EmpName = model.EmpName;
            emp.NickName = model.NickName;
            emp.Contact = model.Contact;
            emp.Role = model.Role;
            emp.Salary = model.Salary.GetValueOrDefault();

            db.Entry(emp).State = EntityState.Modified;
            db.SaveChanges();
            returnModel.Status = 0;
            return Ok(returnModel);
        }

        [HttpPost]
        public IActionResult Login([FromBody] StaffLoginModel emp)
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ReturnModel model = new();
            Employee? employee = (from em in db.Employees
                                  where emp.EmpAccount == em.EmpAccount
                                  select em).SingleOrDefault();
            if (employee == null)
            {
                model.Result = false;
                return Ok(model);
            }
            string password = hash.GetHash(string.Concat(emp.EmpPassword, employee.Salt).ToString());

            if (employee != null && password == employee.EmpPassword && employee.Status != 0)
            {
                 List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.Name,employee.EmpName),
                    new Claim(ClaimTypes.Role,employee.Role),
                };
                ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                model.Result = true;
                model.Object = employee.NickName;
                HttpContext.Session.SetString("nick", employee.NickName);
                return Ok(model);
            }
            model.Result = false;
            return Ok(model);
        }

        [Authorize(Roles = "mod,leader,staff")]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [Authorize(Roles = "leader,mod")]
        [HttpPost]
        public IActionResult Register([FromBody] StaffRegisterModel staff)
        {
            ReturnModel returnModel = new();
            if (!ModelState.IsValid)
            {
                returnModel.Status = 2;
                return Ok(returnModel);
            }
            if (db.Employees.Any(emp => emp.EmpAccount == staff.EmpAccount))
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

        [Authorize(Roles = "mod,leader")]
        [HttpPut]
        public IActionResult StaffGone([FromBody] string acc)
        {
            Employee emp = db.Employees.Select(e => e).Where(e => e.EmpAccount == acc).FirstOrDefault();
            emp.Status = 0;
            emp.Leave = DateTime.Now;
            db.Entry(emp).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(true);
        }

        [Authorize(Roles = "mod")]
        [HttpPost]
        public IActionResult ResetPw([FromBody] StaffLoginModel emp)
        {
            var user = db.Employees.Select(e => e).Where(e => e.EmpAccount == emp.EmpAccount).FirstOrDefault();
            if (user == null)
            {
                return Ok(false);
            }
            user.EmpPassword = hash.GetHash(String.Concat(emp.EmpPassword, user.Salt));
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(true);
        }
    }
}
