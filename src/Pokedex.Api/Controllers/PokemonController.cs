using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Api.Dto.Request;
using Pokedex.Domain;
using Pokedex.Domain.CustomExceptions;

namespace Pokedex.Api.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	[Produces("application/json")]
	public class PokemonController : ControllerBase
	{
		private readonly IMediator _mediator;
		readonly ILogger<PokemonController> _logger;

		public PokemonController(IMediator mediator, ILogger<PokemonController> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}

		[HttpGet]
		[Route("{name}/basic")]
		[ProducesResponseType(typeof(bool), 200)]
		[ProducesResponseType(typeof(ClientErrorData), 400)]
		[ProducesResponseType(typeof(ClientErrorData), 404)]
		public async Task<IActionResult> Get([FromRoute] string name)
		{
			try
			{
				_logger.LogInformation($"Request received for getting basic pokemon information for: {name}");
				var pokemonInfo = await _mediator.Send(new GetPokemonRequest { Name = name, Type = PokemonInfoType.BASIC });
				return new OkObjectResult(pokemonInfo);
			}
			catch (NotFoundException ex)
			{
				return new NotFoundObjectResult(ex.Message);
			}
		}

		[HttpGet]
		[Route("{name}/translated")]
		[ProducesResponseType(typeof(bool), 200)]
		[ProducesResponseType(typeof(ClientErrorData), 400)]
		[ProducesResponseType(typeof(ClientErrorData), 404)]
		public async Task<IActionResult> GetTranslated([FromRoute]  string name)
		{
			try
			{
				_logger.LogInformation($"Request received for getting translated pokemon information for: {name}");
				var pokemonInfo = await _mediator.Send(new GetPokemonRequest { Name = name, Type = PokemonInfoType.TRANSLATED });
				return new OkObjectResult(pokemonInfo);
			}
			catch (NotFoundException ex)
			{
				return new NotFoundObjectResult(ex.Message);
			}
		}
	}
}