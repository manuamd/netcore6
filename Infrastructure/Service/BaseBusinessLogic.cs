using Axiom.Application.Interfaces.Database;
using Axiom.Application.Interfaces.Service;
using Axiom.Infrastructure.Contexts;
using Axiom.Infrastructure.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Axiom.Infrastructure.Service
{
    public class BaseBusinessLogic
    {
        protected IDapper _dapper;

        protected readonly AxiomDBContext _context;
        protected string _connectionString => _userService.ConnectionString;
        protected string _securityConnectionString => _userService.SecurityConnectionString;

        protected readonly IConfiguration _config;

        protected readonly ICurrentUserService _userService;
        protected string userName => _userService.UserId;
        protected string customerKey => _userService.CustomerKey;

        protected readonly IHttpContextAccessor _httpContext;
        public BaseBusinessLogic(IDataContext dataContext)
        {
            _dapper = dataContext.Dapper;
            _context = dataContext.Context;
            _config = dataContext.Config;
            _httpContext = dataContext.HttpContextAccessor;
            _userService = dataContext.UserService;
        }

        public void ifIsNotNullAdd(List<Tuple<string, string>> list, string Name, string Value)
        {
            if (Value != null && Value != "")
            {
                list.Add(new Tuple<string, string>(Name, Value));
            }
            else
            {
                list.Add(new Tuple<string, string>(Name, "-=-"));
            }
        }
    }
}
