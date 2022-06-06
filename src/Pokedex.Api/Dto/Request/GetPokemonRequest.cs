using MediatR;
using Pokedex.Api.Dto.Response;
using Pokedex.Domain;

namespace Pokedex.Api.Dto.Request
{
    public class GetPokemonRequest : IRequest<PokemonInfoResponse>
    {
        public string Name { get; set; }

        public PokemonInfoType Type { get; set; }
    }
}
