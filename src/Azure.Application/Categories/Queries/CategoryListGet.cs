
using Azure.Application.Categories.DTOs;
using Azure.Domain;
using Azure.Persistence;
using Core.Mappy.Interfaces;
using Core.MediatorOR.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Azure.Application.Categories.Queries
{
    public class CategoryListGet
    {
        public class Query: IRequest<List<CategoryResponse>>
        {
           
        }

        public class Handler(AzureDbContext context, IMapper mapper, ILogger<CategoryListGet> logger) : IRequestHandler<Query, List<CategoryResponse>>
        {
            private readonly AzureDbContext _context = context;
            private readonly IMapper _mapper = mapper;
            private readonly ILogger<CategoryListGet> _logger = logger;

            public async Task<List<CategoryResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
              var categories = await _context.Categories.ToListAsync();
              var result = _mapper.Map<List<CategoryResponse>>(categories);
                return result;
            }
        }
    }}
