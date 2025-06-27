
using Core.MediatorOR.Contracts;
using Microsoft.Extensions.Logging;

namespace Azure.Application.Abstractions
{
    public class LogginBehaviors<TRequest, TResponse>(ILogger<LogginBehaviors<TRequest, TResponse>> logger)
        : IPipelineBehaviors<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LogginBehaviors<TRequest, TResponse>> _logger = logger;
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation($"Starting request of type: {typeof(TRequest).Name}");
            var response = await next();
            _logger.LogInformation($"End request of type: {typeof(TRequest).Name}");
            return response;
        }
    }

}
