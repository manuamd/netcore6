using Axiom.Infrastructure.Models;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace Axiom.Application.Interfaces.Service
{
    public interface IBusinessLogic
   {
        Task<ExpandoObject> GetClientInfo(string clientId);
        Task Save();
        Task<string> Login(string username, string password);

        Task<List<SecurityPageModel>> GetUserSpecificSecurity();
        void SendEmailAsync(string userName);
    }
}
