using Axiom.Application;
using Axiom.Infrastructure.Contexts;
using System.Reflection;
namespace AxiomClient.Extensions.ServiceCollection
{
    internal static class ServiceCollectionExtension
    {
        //internal static IServiceCollection AddScopeServices(this IServiceCollection services)
        //{

        //}

        internal static IServiceCollection AddDatabase(this IServiceCollection services)
   => services
       .AddDbContext<AxiomDBContext>()
       .AddDbContext<SecurityDBContext>();
    }
}
