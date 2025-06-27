
using Core.Mappy.Extensions;
using Core.MediatorOR;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Azure.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAzureApplication(this IServiceCollection services)
        {
            services.AddMediatorOR(typeof(DependencyInjection).Assembly);

            services.AddMapper();
            //para hacer automitico las validaciones 
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}
