using Pokedex.Domain.Services;

namespace Pokedex.Domain.Factories
{
    public interface IPokemonInfoServiceFactory
    {
        IPokemonInfoService Create(PokemonInfoType type);
    }
}