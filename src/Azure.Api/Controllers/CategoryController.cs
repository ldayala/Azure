using Azure.Domain;
using Azure.Persistence;
using Core.MediatorOR.Contracts;
using Microsoft.AspNetCore.Mvc;

using static Azure.Application.Categories.Queries.CategoryListGet;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Azure.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AzureDbContext _context;
        private readonly IMediator _mediator;

        public CategoryController(AzureDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<List<Category>> Get(CancellationToken cancellationToken)
        {
            var query = new Query();
           return await _mediator.Send(query,cancellationToken);

        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CategoryController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
