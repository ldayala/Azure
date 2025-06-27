

using Azure.Application.Coffes.DTOs;
using Azure.Persistence;
using Core.Mappy.Interfaces;
using Core.MediatorOR.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Azure.Application.Coffes.Queries
{
    public class CoffeListGet
    {
        public class Query : IRequest<List<CoffeResponse>>
        { }

        public class Handler(AzureDbContext context, IMapper mapper) : IRequestHandler<Query, List<CoffeResponse>>
        {
            private readonly AzureDbContext _context = context;
            private readonly IMapper _mapper = mapper;
            public async Task<List<CoffeResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var coffes = await _context.Coffes
                    .Include(c=>c.Category)
                    .Include(c=>c.Ingredients)
                    .ToListAsync(cancellationToken);
                return _mapper.Map<List<CoffeResponse>>(coffes);
            }
        }

    }
  
}
