
using Core.MediatorOR;
using Microsoft.Extensions.DependencyInjection;

namespace Azure.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAzureApplication(this IServiceCollection services)
        {
            services.AddMediatorOR(typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}
