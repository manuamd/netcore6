using AutoWrapper.Wrappers;
using Axiom.Application.Interfaces.Database;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Axiom.WebApi.Extensions.Middlewares
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TransactionAttribute : Attribute
    {
    }

    public class DbTransactionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;
        public DbTransactionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _loggerFactory = loggerFactory;
        }

        public async Task Invoke(HttpContext httpContext, IDapper connectionProvider)
        {
            var logger = _loggerFactory.CreateLogger<DbTransactionMiddleware>();
            var endpoint = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<TransactionAttribute>();

            // For HTTP GET opening transaction is not required
            if (httpContext.Request.Method.Equals("GET", StringComparison.CurrentCultureIgnoreCase) && attribute == null)
            {
                await _next(httpContext);
                return;
            }

            IDbTransaction transaction = null;

            try
            {
                connectionProvider.TransactionConnectionString = "";
                transaction = connectionProvider.TransactionScope;

                await _next(httpContext);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                if (ex is FileNotFoundException)
                {
                    httpContext.Response.StatusCode = 400;
                }
                else if (ex is ApiException)
                {
                    httpContext.Response.StatusCode = 400;
                }
                else
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
                await httpContext.Response.WriteAsync(ex.Message);
            }
            finally
            {
                transaction?.Dispose();
            }
        }
    }
}
