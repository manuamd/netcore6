using Axiom.Application.Configurations;
using Axiom.Application.Constants;
using Axiom.Application.Interfaces.Service;
using Axiom.Infrastructure.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NetCore.AutoRegisterDi;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Axiom.WebApi.Extensions.ServiceCollection
{
    internal static class ServiceCollectionExtension
    {
        internal static AppConfiguration GetApplicationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationSettingsConfiguration = configuration.GetSection(nameof(AppConfiguration));
            services.Configure<AppConfiguration>(applicationSettingsConfiguration);
            return applicationSettingsConfiguration.Get<AppConfiguration>();
        }

        internal static IServiceCollection AddScopeServices(this IServiceCollection services)
        {
            //var repositoryInterfaces = typeof(Application.IAssemblySelector).Assembly
            // .GetTypes()
            // .Where(item => item.GetInterfaces().Any(i => 
            //    i.GetTypeInfo() == typeof(IScopedDependency)))
            // .ToList();

            //var repositoryClasses = typeof(Infrastructure.IAssemblySelector).Assembly
            //.GetTypes()
            //.Where(item => item.IsAbstract == false && item.IsGenericType == false
            //    && item.GetInterfaces().Any(x => x.GetTypeInfo() == typeof(IScopedDependency)))
            //.ToList();

            //foreach (var repoInterface in repositoryInterfaces)
            //{
            //    var implemetedClass = repositoryClasses.Where(x => x.GetInterfaces().Any(y => y == repoInterface)).FirstOrDefault();

            //    if (implemetedClass != null)
            //    {
            //        services.AddScoped(repoInterface, implemetedClass);
            //    }
            //}

            var assembliesToScan = new[]
               {
                    Assembly.GetAssembly(typeof(Application.IAssemblySelector)),
                    Assembly.GetAssembly(typeof(Infrastructure.IAssemblySelector)),
                    //Assembly.GetAssembly(typeof(IScopedDependency))
               };
            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan).Where(x => x.GetInterfaces().Any(i =>
                i.GetTypeInfo() == typeof(IScopedDependency)))
              .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            return services;
        }


        internal static IServiceCollection AddServices(this IServiceCollection services)
        {
            var assembliesToScan = new[]
              {
                    Assembly.GetAssembly(typeof(Infrastructure.IAssemblySelector))
               };
            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
              .Where(c => c.Name.EndsWith("BusinessLogic"))  //optional
              //.IgnoreThisInterface<IMyInterface>()     //optional
              .AsPublicImplementedInterfaces();

            return services;
        }

        internal static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    => services
        .AddDbContext<AxiomDBContext>()
        .AddDbContext<SecurityDBContext>();

        internal static IServiceCollection AddJwtAuthentication(
           this IServiceCollection services, AppConfiguration config)
        {
            var key = Encoding.ASCII.GetBytes(config.Secret);
            services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(async bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RoleClaimType = ClaimTypes.Role,
                        ClockSkew = TimeSpan.Zero
                    };

                    bearer.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = c =>
                        {
                            if (c.Exception is SecurityTokenExpiredException)
                            {
                                c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                c.Response.ContentType = "application/json";
                                var result = "The Token is expired.";
                                return c.Response.WriteAsync(result);
                            }
                            else
                            {
                                c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                c.Response.ContentType = "application/json";
                                var result = "An unhandled error has occurred.";
                                return c.Response.WriteAsync(result);
                            }
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            if (!context.Response.HasStarted)
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                context.Response.ContentType = "application/json";
                                var result = "You are not Authorized.";
                                return context.Response.WriteAsync(result);
                            }

                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            context.Response.ContentType = "application/json";
                            var result = "You are not authorized to access this resource.";
                            return context.Response.WriteAsync(result);
                        },
                    };
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy(KeyConstants.PolicyAuthoApiUser, policy => policy.RequireClaim(KeyConstants.Strings.JwtClaims.ApiAccess));
            });
            return services;
        }
    }
}
