

using Core.MediatorOR.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Scrutor;

namespace Core.MediatorOR
{
    public static class ServiceCollectiomExtension
    {
        public static IServiceCollection AddMediatorOR(
            this IServiceCollection services,
            params Assembly[] assemblies)
        {
            services.AddScoped<IMediator, Mediator>();

            services.Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());
            return services;
        }
    }
}
