using Pokedex.Domain.Models;

namespace Pokedex.Domain.Interfaces
{
    public interface IPokeApiClient
    {
        Task<BasicPokemonInfoResponse> GetBasicInformation(string name);
    }
}