using Axiom.Application.Interfaces.Service;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Axiom.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<AccountController> _logger;
        private IBusinessLogic _businessLogic;
        private readonly IConfiguration _configuration;
        public AccountController(ILogger<AccountController> logger, IBusinessLogic businessLogic, IConfiguration configuration)
        {
            _logger = logger;
            _businessLogic = businessLogic;
            _configuration = configuration;
        }

        [HttpGet("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login()
        {
            string jwt = await _businessLogic.Login("tpham", "HIMSIs#1");
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(10);
            
            Response.Cookies.Append("jwt", jwt, option);
            return Ok(jwt);
        }

        [HttpGet("SendEmail")]
        [AllowAnonymous]
        public async Task<ActionResult> SendEmail()
        {
            var result = string.Empty;
            var jobId = BackgroundJob.Schedule(() => _businessLogic.SendEmailAsync("hle"), TimeSpan.FromMinutes(2));
            return Ok($"Job Id {jobId} Completed. Welcome Mail Sent!");
        }

    }
}
