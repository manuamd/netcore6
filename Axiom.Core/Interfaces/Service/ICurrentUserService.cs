using Axiom.Application;

namespace Axiom.Application.Interfaces.Service
{
    public interface ICurrentUserService : IScopedDependency
    {
        string UserId { get; }
        string CustomerKey { get; }
        string ConnectionString { get; }
        string SecurityConnectionString { get; }
    }
}
