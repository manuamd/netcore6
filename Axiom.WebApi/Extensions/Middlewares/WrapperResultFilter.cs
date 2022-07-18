using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Axiom.WebApi.Extensions.Middlewares
{
    public class WrapperResultFilter
    {
        //public override void OnResultExecuting(ResultExecutingContext context)
        //{
        //    base.OnResultExecuting(context);

        //    var result = context.Result as ObjectResult;
        //    var apiResponse = new ApiResponseModel
        //    {
        //        Data = result?.Value,
        //        StatusCode = (HttpStatusCode)(result?.StatusCode ?? 200),
        //        Message = "Success",
        //        ErrorDetails = null,
        //        ModelValidationErrors = null,
        //    };

        //    context.Result = new ObjectResult(apiResponse)
        //    {
        //        StatusCode = result?.StatusCode ?? 200
        //    };
        //}
    }
}
