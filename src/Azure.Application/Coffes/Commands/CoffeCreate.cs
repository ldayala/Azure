
using Azure.Application.Abstractions;
using Azure.Application.Coffes.DTOs;
using Azure.Domain;
using Azure.Persistence;
using Core.Mappy.Interfaces;
using Core.MediatorOR.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Azure.Application.Coffes.Commands
{
    public class CoffeCreate
    {
        public class Command : IRequest<Result<Guid>>
        {
            public required CoffeCreateRequest CoffeCreateRequest { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.CoffeCreateRequest).SetValidator(new RequestValidator());
            }
        }
        public class RequestValidator : AbstractValidator<CoffeCreateRequest>
        {
            public RequestValidator()
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
                RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
                RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("CategoryId must be greater than zero.");
            }
        }
        public class Handler(
            AzureDbContext dbContext,
            IMapper mapper)
            : IRequestHandler<Command, Result<Guid>>
        {
            private readonly AzureDbContext dbContext = dbContext;
            private readonly IMapper _mapper = mapper;


            public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var existCoffe = await dbContext.Coffes.AnyAsync(c => c.Name == request.CoffeCreateRequest.Name);

                if (existCoffe)
                    return Result<Guid>.Failure(new Error("Coffe already exists", "Coffe.Existe"));

                var coffe = _mapper.Map<Coffe>(request.CoffeCreateRequest);
                dbContext.Add(coffe);
                await dbContext.SaveChangesAsync(cancellationToken);
                return Result<Guid>.Success(coffe.Id);
            }
        }
    }
}
