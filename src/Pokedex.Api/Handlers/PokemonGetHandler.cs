using AutoMapper;
using MediatR;
using Pokedex.Api.Dto.Request;
using Pokedex.Api.Dto.Response;
using Pokedex.Domain.Factories;

namespace Pokedex.Api.Handlers
{
    public class PokemonGetHandler : IRequestHandler<GetPokemonRequest, PokemonInfoResponse>
    {
        private readonly IPokemonInfoServiceFactory _pokemonServiceFactory;
        private readonly IMapper _mapper;

        public PokemonGetHandler(IPokemonInfoServiceFactory pokemonServiceFactory, IMapper mapper)
        {
            _pokemonServiceFactory = pokemonServiceFactory;
            _mapper = mapper;
        }

        public async Task<PokemonInfoResponse> Handle(GetPokemonRequest request, CancellationToken cancellationToken)
        {
            var pokemonservice = _pokemonServiceFactory.Create(request.Type);
            var pokemonInfo = await pokemonservice.GetPokemonInfo(request.Name);
            return _mapper.Map<PokemonInfoResponse>(pokemonInfo);
        }
    }
}
