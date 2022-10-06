﻿using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alcoholic.Controllers.API
{
    [Route("api/[action]")]
    [ApiController]
    public class MemberApiController : ControllerBase
    {
        private readonly db_a8de26_projectContext _projectContext;
        public MemberApiController(db_a8de26_projectContext projectContext)
        {
            _projectContext = projectContext;
        }

        // "api/MemberApi"
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMember()
        {
            return await _projectContext.Members.ToListAsync();
        }

        // "api/Login"
        [HttpPost]
        public string Login(Member memberData)
        {
            Member? user = (from member in _projectContext.Members
                                  where member.MemberAccount == memberData.MemberAccount
                                  && member.MemberPassword == memberData.MemberPassword
                                  select member).SingleOrDefault();

            if (user == null)
            {
                return "帳號密碼錯誤";
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

                return "LoginSuccess";
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
        public async Task<ActionResult<Member>> Register(Member memberData)
        {
            int number = _projectContext.Members.Count() + 100;
            memberData.MemberId = DateTime.Now.ToString("yyyyMMdd") + number.ToString();
            await _projectContext.AddAsync(memberData);
            await _projectContext.SaveChangesAsync();
            return memberData;
        }



        public string NoAccess()
        {
            return "沒有權限";
        }

        public string NoLogin()
        {
            return "尚未登入";
        }

        [HttpPost]
        public string Test(Member memberData)
        {
            Console.WriteLine(memberData);
            return " ";
        }
    }
}
