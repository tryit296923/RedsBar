using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using Alcoholic.Services;

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
        public List<Employee> GetAllMember()
        {
            return db.Employees.ToList();
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
        [Authorize(Roles = "moderater")]
        [HttpPost]
        public IActionResult Register([FromBody] Employee emp)
        {
            if (emp == null)
            {
                emp.Salt = Guid.NewGuid().ToString("N");
                emp.EmpPassword = hash.GetHash((string)emp.EmpPassword.Concat(emp.Salt));
                db.Add(emp);
                db.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

    }
}
