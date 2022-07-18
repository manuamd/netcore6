using Hangfire.Dashboard;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security;
using static Axiom.Application.Constants.KeyConstants.Strings;

namespace Axiom.WebApi.Permission
{
    public class HangfireDashboardJwtAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public HangfireDashboardJwtAuthorizationFilter() 
        {
        }

        public bool Authorize(DashboardContext context)
        {

            if (Debugger.IsAttached)
            {
                return true;
            }


            try
            {
                var httpContext = context.GetHttpContext();
                var jwtToken = httpContext.Request.Cookies["jwts"];
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(jwtToken);
                return jwtSecurityToken.Claims.Any(t => t.Type == JwtClaimIdentifiers.Rol && t.Value == JwtClaims.ApiAccess);
                // Only authenticated users who have the required claim (AzureAD group in this demo) can access the dashboard.
                //return httpContext.User.Identity.IsAuthenticated;
            }
            catch (Exception exception)
            {
                return false;
            }
        }
    }
}
