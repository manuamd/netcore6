using Axiom.Application.Interface.Service;
using Axiom.Application.Interfaces.Database;
using Axiom.Application.Interfaces.Service;
using Axiom.Infrastructure.Contexts;
using Axiom.Infrastructure.Database;
using Axiom.Infrastructure.Interface.Service;
using Axiom.Infrastructure.Service;
using AxiomClient;
using AxiomClient.Extensions.ServiceCollection;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddDatabase();
builder.Services.AddHttpContextAccessor();
builder.Services.AddLazyCache();
builder.Services.AddScoped<IDapper, DapperService>();
builder.Services.AddScoped<IDataContext, DataContext>();
builder.Services.AddScoped<IBusinessLogic, BusinessLogic>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
await builder.Build().RunAsync();
