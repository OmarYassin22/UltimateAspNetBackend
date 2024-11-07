using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Api_New_Feature.ActionFitler
{
    public class AyncActionFilterExample : IAsyncActionFilter
    {
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Debug.WriteLine("Action Executing");
            var result = next();
          
            Debug.WriteLine("Action Executed");
            return result;
        }
    }
}
