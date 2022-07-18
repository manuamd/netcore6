using AutoWrapper;
using Axiom.Application.Configurations;
using Axiom.Application.Interfaces.Service;
using Axiom.Infrastructure.Extension;
using Axiom.WebApi.Extensions.Middlewares;
using Axiom.WebApi.Extensions.ServiceCollection;
using Axiom.WebApi.Permission;
using Hangfire;
using Hangfire.Dashboard;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace Axiom.WebApi
{
    public class Startup
    {
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:5275")
                                     .AllowAnyMethod()
                                     .AllowAnyHeader()
                                     .AllowCredentials();
                                  });
            });
            services.AddControllers().AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            services.AddDatabase(Configuration);
            services.AddScoped<IScopedDependency, ScopedDependency>();
            services.Configure<AppConfiguration>(
                Configuration.GetSection("AppConfiguration"));
            services.AddJwtAuthentication(services.GetApplicationSettings(Configuration));
            services.AddHttpContextAccessor();
            services.AddScopeServices();
            services.AddServices();
            services.AddInfrastructureLayer();
            services.AddLazyCache();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "JWTToken_Auth_API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        }, new List<string>()
                    },
                });
            });
            services.AddRazorPages();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddHangfire(x => x.UseSqlServerStorage("Data Source=.;Initial Catalog=NetCoreDb; Persist Security Info=True;User ID=sa;password=sysadmin2012"));
            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { ExcludePaths = new AutoWrapperExcludePath[] { new AutoWrapperExcludePath("/mydashboard/.*|/mydashboard|/ui/.*", ExcludeMode.Regex) }, BypassHTMLValidation = true, ShowStatusCode = true, UseCamelCaseNamingStrategy = false });
            //app.UseExceptionHandler(c => c.Run(async context =>
            //{
            //    var error = context.Features
            //        .Get<IExceptionHandlerPathFeature>()
            //        .Error;

            //    if (error is FileNotFoundException)
            //    {
            //        context.Response.StatusCode = 400;
            //    }


            //    await context.Response.WriteAsync(error.Message);
            //}));
            app.UseHangfireDashboard("/mydashboard", options: new DashboardOptions()
            {
                Authorization = new IDashboardAuthorizationFilter[] {
                    new HangfireDashboardJwtAuthorizationFilter()
                }
            });
            app.UseMiddleware<DbTransactionMiddleware>();
            app.UseBlazorFrameworkFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
