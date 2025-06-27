
namespace Core.MediatorOR.Contracts
{
    public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();
    /*
     esta clase va a tener un handler que se va a ajecutra antes del sqrs
     */
    public interface IPipelineBehaviors<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(
            TRequest request, 
            CancellationToken cancellationToken,
           RequestHandlerDelegate<TResponse> next
            );

    }
}
