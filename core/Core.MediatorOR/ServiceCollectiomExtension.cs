using Core.MediatorOR.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


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

            services.Scan(scan => scan
               .FromAssemblies(assemblies)
               .AddClasses(classes => classes.AssignableTo(typeof(IPipelineBehaviors<,>)))
               .AsImplementedInterfaces()
               .WithTransientLifetime());
            return services;
        }
    }
}
