using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Alcoholic.Areas.BackCenter.Controllers
{
    [Authorize(Roles = "Moderater")]
    public class BackController : Controller
    {
        private readonly db_a8de26_projectContext db;
        private readonly HashService hash;
        public BackController(db_a8de26_projectContext db, HashService hash)
        {
            this.db = db;
            this.hash = hash;
        }

        [Authorize(Roles = "moderater,leader,staff")]
        public IActionResult BackIndex()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginPage()
        {
            return View();
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
