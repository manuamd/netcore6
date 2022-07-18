using Axiom.Application.Configurations;
using Axiom.Application.Interfaces.Service;
using Axiom.Infrastructure.Contexts;
using Axiom.Infrastructure.Interface;
using Axiom.Infrastructure.Models;
using LazyCache;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Axiom.Application.Constants.KeyConstants.Strings;

namespace Axiom.Infrastructure.Service
{
    public class BusinessLogic : BaseBusinessLogic, IBusinessLogic
    {
        private SecurityDBContext _securityDBContext;
        private readonly AppConfiguration _appConfig;
        private readonly IAppCache _cache;
        private DateTime shortTimeSpan = DateTime.UtcNow.AddMinutes(10);
        public BusinessLogic(SecurityDBContext securityDBContext, IOptions<AppConfiguration> appConfig, IDataContext dataContext, IAppCache cache) : base(dataContext)
        {
            _securityDBContext = securityDBContext;
            _appConfig = appConfig.Value;
            _cache = cache;
        }
        public async Task<ExpandoObject> GetClientInfo(string clientId)
        {
            List<Tuple<string, string>> args = new List<Tuple<string, string>> { new Tuple<string, string>("ClientId", clientId) };
            return await _dapper.ExecuteProcedureAsync(_connectionString, "ClientHeaderInfoAxiom", args);
        }

        public async Task<string> Login(string username, string password)
        {
            try
            {
                //if (pass2.PasswordHash == HashCheck)
                if (1 == 1)
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(JwtClaimsType.UserName, username));
                    claims.Add(new Claim(JwtClaimIdentifiers.Rol, JwtClaims.ApiAccess));

                    return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                            claims: claims,
                            expires: DateTime.UtcNow.AddHours(8),
                            signingCredentials: GetSigningCredentials()
                        ));

                }

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes(_appConfig.Secret);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }

        private string GetSwcSHA1(string value)
        {
            SHA1 algorithm = SHA1.Create();
            byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            string sh1 = "";
            for (int i = 0; i < data.Length; i++)
            {
                sh1 += data[i].ToString("x2").ToUpperInvariant();
            }
            return sh1;
        }

        public async Task Save()
        {
            var test = userName;
            var result = _context.EncounterForms.Select(x => x.FormName).ToList();

            List<Tuple<string, string>> args = new List<Tuple<string, string>>() { };
            ifIsNotNullAdd(args, "ClientId", "88888");
            ifIsNotNullAdd(args, "Username", "tpham");
            ifIsNotNullAdd(args, "NoteValue", "test new axiom");
            ifIsNotNullAdd(args, "EncounterNoteTypeId", "1");
            await _dapper.ProcedureExecuteWithTransactionAsync<string>("ClientHistoryNoteDataWriteAxiom", args);

            List<Tuple<string, string>> args1 = new List<Tuple<string, string>>() { };
            ifIsNotNullAdd(args1, "ClientId", "88888");
            ifIsNotNullAdd(args1, "Username", "tpham");
            ifIsNotNullAdd(args1, "NoteValue", "test new axiom");
            ifIsNotNullAdd(args1, "EncounterNoteTypeId", "dsfdffsdsdfsdfsfdfdssfdsfdsdfsfd");
            await _dapper.ProcedureExecuteWithTransactionAsync<string>("ClientHistoryNoteDataWriteAxiom", args1);

        }

        public async Task<List<SecurityPageModel>> GetUserSpecificSecurity()
        {
            var specSecurity = await _cache.GetOrAddAsync("empIdSpecSecurity", async () =>
            {
                List<Tuple<string, string>> args = new List<Tuple<string, string>>() { };
                ifIsNotNullAdd(args, "EmployeeId", "3513");
                var eObject = await _dapper.ExecuteProcedureAsync(_connectionString, "SpecificSecurityByEmployeeId", args);
                return eObject?.Select(x => new SecurityPageModel(x.Value)).ToList() ?? new List<SecurityPageModel>();
            }, shortTimeSpan);

            return specSecurity;
        }

        public void SendEmailAsync(string userName)
        {
            Console.WriteLine($"Welcome to our application, {userName}");
        }
    }
}
