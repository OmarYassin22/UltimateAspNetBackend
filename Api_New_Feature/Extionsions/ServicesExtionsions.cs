using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Api_New_Feature.ServicesExtionsions
{
    public static class ServicesExtionsions
    {
        public static IServiceCollection ConfigureCROS(this IServiceCollection service)
        {
            service.AddCors(opt =>
            {
                opt.AddPolicy("CrosPolicy", builder =>
                {

                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });

            });
            return service;



        }



        /// <summary>
        /// Extionstion method to enable hoist app on IIS
        /// </summary>
        /// <param name="services"></param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection IISConfiguration(this IServiceCollection services)
        {


            services.Configure<IISOptions>(opt =>
            {
                //opt.AutomaticAuthentication = false;
                //opt.AuthenticationDisplayName = "Windows";
                //opt.AutomaticAuthentication = true;
            });
            return services;
        }

        public static IServiceCollection ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),b=>b.MigrationsAssembly("Api_New_Feature")));
            return services;

        }
    }
}
