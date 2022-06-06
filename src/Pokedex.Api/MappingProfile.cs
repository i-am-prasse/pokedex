using AutoMapper;
using Pokedex.Api.Dto.Response;
using Pokedex.Domain.Models;
using System.Text.RegularExpressions;

namespace Pokedex.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BasicPokemonInfoResponse, PokemonInfo>()
                .ForMember(o => o.Habitat, x => x.MapFrom(y => y.Habitat.Name))
                 .ForMember(o => o.Description, x => x.MapFrom(y => Regex.Replace((y.FlavorTextEntries.FirstOrDefault(x => x.Language.Name == "en").FlavorText), @"\t|\n|\r|\f", " ")));

            CreateMap<PokemonInfo, PokemonInfoResponse>();
        }
    }
}
