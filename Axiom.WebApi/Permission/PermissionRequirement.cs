using Axiom.Application.Interfaces.Service;
using Axiom.Infrastructure.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Axiom.WebApi.Permission
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string type, string claimValue) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(type, claimValue) };
        }
    }

    public class ClaimRequirementFilter : IAsyncAuthorizationFilter
    {
        readonly Claim _claim;
        readonly AxiomDBContext _context;
        readonly IBusinessLogic _businessLogic;
        public ClaimRequirementFilter(Claim claim, AxiomDBContext context, IBusinessLogic businessLogic)
        {
            _claim = claim;
            _context = context;
            _businessLogic = businessLogic;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new ForbidResult();
            }

            else if (_claim.Type == ClaimTypes.Role)
            {
                var permissionRoleIds = _context.convFormSecurity.Where(x => x.Path.ToLower() == _claim.Value).Select(x => x.RoleId.ToString()).ToList();
                var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == ClaimTypes.Role && permissionRoleIds.Contains(c.Value));
                if (!hasClaim)
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                var specSecurities = await _businessLogic.GetUserSpecificSecurity();
                var security = specSecurities.Where(x => x.SpecificSecurityName == _claim.Type).FirstOrDefault();
                switch (_claim.Value)
                {
                    case "UserSave":
                        if (!security.UserSave)
                        {
                            context.Result = new ForbidResult();
                        }
                        break;
                    default:
                        break;

                }
            }
        }
    }

}
