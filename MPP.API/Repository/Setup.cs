
using MPP.API.Service;
using MPP.API.Entities;
namespace MPP.API.Repository
{
    public static class Setup
    {

        public static void RegisterRepository(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<MPPRepository>();
        }

        public static void RegisterService(this WebApplicationBuilder builder)
        {

            // services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));  

            // var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            // builder.Services.AddDbContext<MPPDBContext>(options => options.UseSqlServer(connectionString));

            builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));  
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<MPPService>();

        }
    }
}