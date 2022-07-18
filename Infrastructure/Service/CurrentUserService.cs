using Axiom.Application.Interfaces.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using static Axiom.Application.Constants.KeyConstants.Strings;

namespace Axiom.Infrastructure.Service
{
    public class CurrentUserService : ICurrentUserService, IScopedDependency
    {
        private readonly IConfiguration _config;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _config = config;
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtClaimsType.UserName);
            Claims = httpContextAccessor.HttpContext?.User?.Claims.AsEnumerable().Select(item => new KeyValuePair<string, string>(item.Type, item.Value)).ToList();
            CustomerKey = httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtClaimsType.CustomerKey);
        }

        public string UserId { get; }
        public List<KeyValuePair<string, string>> Claims { get; set; }

        public string CustomerKey { get; }

        public string ConnectionString => !string.IsNullOrWhiteSpace(CustomerKey) ? _config.GetConnectionString("CKey" + CustomerKey) : _config.GetConnectionString("DefaultConnection");

        public string SecurityConnectionString => _config.GetConnectionString("SecurityDBConnection");
    }
}
