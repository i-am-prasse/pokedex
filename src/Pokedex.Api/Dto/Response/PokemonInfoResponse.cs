﻿namespace Pokedex.Api.Dto.Response
{
    public class PokemonInfoResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }
    }
}