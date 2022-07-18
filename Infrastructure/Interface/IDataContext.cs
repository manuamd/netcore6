using Axiom.Application.Interfaces.Service;
using Axiom.Application.Interfaces.Database;
using Axiom.Infrastructure.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace Axiom.Infrastructure.Interface
{
    public interface IDataContext
    {
        IDapper Dapper { get; set; }
        AxiomDBContext Context { get; set; }
        IConfiguration Config { get; set; }
        IHttpContextAccessor HttpContextAccessor { get; set; }
        ICurrentUserService UserService { get; set; }
    }
}
