using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Alcoholic.Services
{
    public class CookieAuthService : CookieAuthenticationEvents
    {

        public CookieAuthService()
        {

        }
        public override Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var principal = context.Principal;
            return base.ValidatePrincipal(context);
        }

    }
}
