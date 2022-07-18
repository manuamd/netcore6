using Axiom.Application.Interfaces.Service;
using Axiom.Infrastructure.Features.Client.Queries;
using Axiom.Infrastructure.Models;
using Axiom.WebApi.Extensions.Middlewares;
using Axiom.WebApi.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Axiom.WebApi.Controllers
{
    //[ClaimRequirement(ClaimTypes.Role, "genericnote")]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private IBusinessLogic _businessLogic;
        private readonly IConfiguration _configuration;
        private IMediator _mediatorInstance;

        protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IBusinessLogic businessLogic, IConfiguration configuration)
        {
            _logger = logger;
            _businessLogic = businessLogic;
            _configuration = configuration;
        }

        [ClaimRequirement("Appointment", "UserSave")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //var client =  await _mediator.Send(new GetClientByIdQuery() { ClientId = "29355" });
            var specSecurities = await _businessLogic.GetUserSpecificSecurity();
            return Ok(specSecurities);
        }

    }
}
