using Pokedex.Domain.Models;

namespace Pokedex.Domain.Services
{
    public interface IPokemonInfoService
    {
        Task<PokemonInfo> GetPokemonInfo(string name);
    }
}