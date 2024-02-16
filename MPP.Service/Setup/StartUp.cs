using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using MPP.Service.Models;

namespace MPP.Service.Setup
{
    public static class StartUp
    {
        #region Register Context
        public static void RegisterContext(this IServiceCollection services, IConfigurationRoot configuration){

            services.Configure<ConnectionStrings>(configuration);
        }

        public static void RegisterContext(this WebApplicationBuilder builder)
        {
            RegisterContext(builder.Services, builder.Configuration);
        }

        #endregion 


        #region Register Service
        public static void RegisterService(this WebApplicationBuilder builder)
        {
            RegisterService(builder.Services, builder.Configuration);
        }

        public static void RegisterService(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddSingleton<IConfiguration>(configuration);

        }
        #endregion

    }
}