
using Azure.Domain;
using Azure.Persistence;
using Core.MediatorOR.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Azure.Application.Categories.Queries
{
    public class CategoryListGet
    {
        public class Query: IRequest<List<Category>>
        {
           
        }

        public class Handler(AzureDbContext context) : IRequestHandler<Query, List<Category>>
        {
            private readonly AzureDbContext _context = context;
            public async Task<List<Category>> Handle(Query request, CancellationToken cancellationToken)
            {
                var categories = await _context.Categories.ToListAsync();
                return categories;
            }
        }
    }
}
