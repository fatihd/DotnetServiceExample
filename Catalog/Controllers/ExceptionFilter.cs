using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Catalog.Service;

namespace Catalog.Controllers
{
    public class ExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is NotFoundException notFoundException)
            {
                context.Result = new ObjectResult(notFoundException.Message)
                {
                    StatusCode = 404
                };

                context.ExceptionHandled = true;
            }
        }
    }
}
