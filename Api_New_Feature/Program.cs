using Api_New_Feature.ActionFitler;
using Api_New_Feature.DataTransfareObject;
using Api_New_Feature.Extionsions;
using Api_New_Feature.ServicesExtionsions;
using Contracts;
using LoggerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Web;
using Repository;
using Repository.DataShaping;



//using NLog;
using System.Reflection.Metadata;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // create instance of NLog  
        LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

        builder.Services.AddControllers(/*x => x.RespectBrowserAcceptHeader = true*/ opt =>
        {
            opt.Filters.Add<ActionFilterExample>();
            opt.Filters.Add(new AyncActionFilterExample());
        })
            //.AddXmlDataContractSerializerFormatters()
            // add newtonsoft json
            .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle  
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // allow cors for all headers,methods and origins  
        builder.Services.ConfigureCROS();
        // enable hoist app on IIS  
        builder.Services.IISConfiguration();
        // enalbe Nlog  
        builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
        // configure sql context  
        builder.Services.ConfigureSqlContext(builder.Configuration);
        // add repository manager / unit of work to IOC  
        builder.Services.AddScoped<IRepositoryManger, RepositoryManager>();
        builder.Services.AddScoped<ICompanyRepository, CompanyRespository>();
        builder.Services.AddScoped<IEmployeeRepository, EmplyeeRespository>();
        // enable validation filter
        builder.Services.AddScoped<ValidationActionFilter>();

        builder.Services.AddScoped<IDataShaper<EmployeeDTO>, DataShaper<EmployeeDTO>>();

        // enable error Unproccable 422 in invalied requests
        builder.Services.Configure<ApiBehaviorOptions>(
            opt => opt.SuppressModelStateInvalidFilter = true);

        var app = builder.Build();

        // Configure the HTTP request pipeline.  
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHsts();
        }

        var logger = app.Services.GetRequiredService<ILoggerManager>();
        // add exception middleware
        app.ConfigureExceptionHandler(logger);

        app.UseHttpsRedirection();
        app.UseCors("CrosPolicy");
        app.UseForwardedHeaders(new ForwardedHeadersOptions()
        {
            ForwardedHeaders = ForwardedHeaders.All
        });
        app.UseAuthorization();
        app.UseStaticFiles();

        app.MapControllers();

        app.Run();
    }
}
