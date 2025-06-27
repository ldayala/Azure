
using Azure.Application.Exception;
using Core.MediatorOR.Contracts;
using FluentValidation;
using FluentValidation.Results;

namespace Azure.Application.Abstractions
{
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehaviors<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            // Implement validation logic here
            // For example, you could use FluentValidation or any other validation library

            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request); //obtenemos e contexto de validacion

            var failures = _validators
                .Select(v => v.Validate(context)) //validamos el contexto con cada validador
                .Where(result => result.Errors.Any()) //filtramos los resultados que tienen errores
                .SelectMany(result => result.Errors) //obtenemos los errores de cada resultado
                .Select(ValidationFailures=> new ValidationFailure(
                    ValidationFailures.PropertyName,
                    ValidationFailures.ErrorMessage))
                .ToList(); //convertimos a lista

            if (failures.Any())
            {
                // Throw an exception or handle the validation failures as needed
                throw new ValidationException(failures);
            }

            return await next(); // If validation passes, continue to the next handler
        }
    }
}
