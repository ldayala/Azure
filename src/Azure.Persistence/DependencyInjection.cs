
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Azure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAzurePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AzureDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("SqliteDatabase"));
            });
            return services;
        }
    }
}
