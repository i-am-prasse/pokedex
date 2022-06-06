using Pokedex.Domain.Services;

namespace Pokedex.Domain.Factories
{
    public class PokemonInfoServiceFactory : IPokemonInfoServiceFactory
    {
        private readonly Dictionary<PokemonInfoType, Func<IPokemonInfoService>> _pokemonInfoServices;

        public PokemonInfoServiceFactory(Dictionary<PokemonInfoType, Func<IPokemonInfoService>> pokemonInfoServices) => _pokemonInfoServices = pokemonInfoServices;

        public IPokemonInfoService Create(PokemonInfoType type)
        {
            if (!_pokemonInfoServices.TryGetValue(type, out var pokemonInfoService) || pokemonInfoService is null)
                throw new ArgumentOutOfRangeException(nameof(type), $"Pokemon Info type '{type}' is not registered");
            return pokemonInfoService();
        }
    }
}
