using Azure.Application.Coffes.Commands;
using Azure.Application.Coffes.DTOs;
using Core.Mappy.Interfaces;
using Core.MediatorOR.Contracts;
using Microsoft.AspNetCore.Mvc;

using static Azure.Application.Coffes.Queries.CoffeListGet;

namespace Azure.Api.Controllers
{
    [Route("api/coffes")]
    [ApiController]
    public class CoffeController(IMediator mediator, IMapper mapper) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<List<CoffeResponse>> GetCoffes(CancellationToken cancellationToken)
        {
            var query = new Query();
            var result = await _mediator.Send(query, cancellationToken);
            return _mapper.Map<List<CoffeResponse>>(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoffe(CoffeCreateRequest coffeCreateRequest, CancellationToken cancellationToken)
        {
            var comand = new CoffeCreate.Command { CoffeCreateRequest = coffeCreateRequest };
            var result=await _mediator.Send(comand, cancellationToken);
            if(result.IsSuccess )
            return Created($"api/coffes/{result.Value}",result.Value);

            return BadRequest(result.Errors);

        }
    }
}
