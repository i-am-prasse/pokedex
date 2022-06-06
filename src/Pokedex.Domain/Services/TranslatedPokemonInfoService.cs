using AutoMapper;
using Microsoft.Extensions.Logging;
using Pokedex.Domain.CustomExceptions;
using Pokedex.Domain.Interfaces;
using Pokedex.Domain.Models;
using System.Text.RegularExpressions;

namespace Pokedex.Domain.Services
{
    public class TranslatedPokemonInfoService : IPokemonInfoService
    {
        private readonly IPokeApiClient _pokeApiClient;
        private readonly IFunTranslationClient _funTranslationClient;
        private readonly IMapper _mapper;
        private readonly ILogger<TranslatedPokemonInfoService> _logger;
        private const string Cave = "cave";
        private const string Yoda = "yoda";
        private const string Shakespeare = "shakespeare";
        private const string En = "en";


        public TranslatedPokemonInfoService(IPokeApiClient pokeApiClient, IMapper mapper, IFunTranslationClient funTranslationClient, ILogger<TranslatedPokemonInfoService> logger)
        {
            _pokeApiClient = pokeApiClient;
            _mapper = mapper;
            _funTranslationClient = funTranslationClient;
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

            var pokemonInfo = _mapper.Map<PokemonInfo>(basicPokemonInfoResponse);

            string type;

            type = (basicPokemonInfoResponse.Habitat.Name.ToLower() == Cave || basicPokemonInfoResponse.IsLegendary) ? Yoda: Shakespeare;
                    
            pokemonInfo.Description = await _funTranslationClient.GetTranslation(Regex.Replace(basicPokemonInfoResponse.FlavorTextEntries.FirstOrDefault(x => x.Language?.Name == En)?.FlavorText, @"\t |\n |\r |\f", " "),type);

            return pokemonInfo;
        }
    }
}
