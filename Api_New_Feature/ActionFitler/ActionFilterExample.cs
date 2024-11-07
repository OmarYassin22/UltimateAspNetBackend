using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Api_New_Feature.ActionFitler
{
    public class ActionFilterExample : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
           Debug.WriteLine("Action Executed");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Debug.WriteLine("Action Executing");
        }
    }
}
