using Checkout.API.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Net;
using System.Net.Http;

namespace Checkout.API.Filters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
                Log.Warning("Model is invalid!");
            }

        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
