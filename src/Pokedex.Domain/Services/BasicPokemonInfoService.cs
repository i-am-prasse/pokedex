using AutoMapper;
using Microsoft.Extensions.Logging;
using Pokedex.Domain.CustomExceptions;
using Pokedex.Domain.Interfaces;
using Pokedex.Domain.Models;

namespace Pokedex.Domain.Services
{
    public class BasicPokemonInfoService : IPokemonInfoService
    {
        private readonly IPokeApiClient _pokeApiClient;
        private readonly IMapper _mapper;
        private readonly ILogger<BasicPokemonInfoService> _logger;

        public BasicPokemonInfoService(IPokeApiClient pokeApiClient, IMapper mapper, ILogger<BasicPokemonInfoService> logger)
        {
            _pokeApiClient = pokeApiClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PokemonInfo> GetPokemonInfo(string name)
        {
            var basicPokemonInfoResponse = await _pokeApiClient.GetBasicInformation(name);

            if (basicPokemonInfoResponse == null)
            {
                _logger.LogError($"There was an exception, Pokemon named '{name}' does not exist ");
                throw new NotFoundException($"The Pokemon {name} does not exist.");
            }

            return _mapper.Map<PokemonInfo>(basicPokemonInfoResponse);
        }
    }
}
