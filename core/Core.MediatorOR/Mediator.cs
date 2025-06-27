
using Core.MediatorOR.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Core.MediatorOR
{
    public class Mediator : IMediator
    {
        //el service provide me permite crear instancias de objetos dinamicamente utilizando el mecanismo reflection de .NET
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            //obtenemos el tipo de handler que maneja la peticion
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            dynamic handler = _serviceProvider.GetRequiredService(handlerType);

            if (handler == null)
            {
                throw new InvalidOperationException($"Handler for request type {request.GetType().Name} not found.");
            }
            //llammos al delegate y los behaviors
            var behaviorType = typeof(IPipelineBehaviors<,>).MakeGenericType(request.GetType(), typeof(TResponse));

            var behaviors = _serviceProvider.GetServices(behaviorType).Cast<dynamic>().Reverse().ToList();

            RequestHandlerDelegate<TResponse> handlerDelegate = () => handler.Handle((dynamic)request, cancellationToken);

            foreach (var behavior in behaviors)
            {
                var next = handlerDelegate;
                handlerDelegate = () => behavior.Handle((dynamic)request, cancellationToken, next);
            }
            //llamamos al metodo handle del handler que maneja la peticion
            // return await handler.Handle((dynamic)request, cancellationToken).ConfigureAwait(false);
            return await handlerDelegate();
        }
    }
}
