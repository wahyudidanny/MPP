
using MPP.API.Service;
using Microsoft.EntityFrameworkCore;

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

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<MPPDBContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<MPPService>();

        }
    }
}