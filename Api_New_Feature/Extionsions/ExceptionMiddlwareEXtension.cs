using Contracts;
using Entities.Eorror_Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Api_New_Feature.Extionsions
{
    public static class ExceptionMiddlwareEXtension
    {

        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {

                appError.Run(async context =>
                {

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var feature = context.Features.Get<IExceptionHandlerFeature>();
                    if (feature != null)
                    {
                        logger.LogError($"Something went wrong: {feature.Error}");

                        await context.Response.WriteAsync(
                        new ErrorDetails()
                        {
                            Message = "Internal Server Error",
                            StatusCode = context.Response.StatusCode
                        }.ToString()
                        );
                    }
                });

            });

        }
    }
}
