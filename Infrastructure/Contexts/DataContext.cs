using Axiom.Application.Interfaces.Database;
using Axiom.Application.Interfaces.Service;
using Axiom.Infrastructure.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Axiom.Infrastructure.Contexts
{
    public class DataContext: IDataContext, IScopedDependency
    {
        public IDapper Dapper { get; set; }
        public AxiomDBContext Context { get; set; }
        public IConfiguration Config { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        public ICurrentUserService UserService { get; set; }

        public DataContext(IDapper dapper, AxiomDBContext context, IConfiguration config, IHttpContextAccessor httpContextAccessor, ICurrentUserService userService)
        {
            Dapper = dapper;
            Context = context;
            Config = config;
            HttpContextAccessor = httpContextAccessor;
            UserService = userService;
        }
    }
}
