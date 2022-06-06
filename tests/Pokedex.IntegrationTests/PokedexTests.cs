using FluentAssertions;
using Pokedex.Api.Dto.Response;
using System.Net.Http.Json;

namespace Pokedex.IntegrationTests
{
    public class PokedexTests
    {
        [Theory]
        [InlineData("mewtwo")]

        public async Task Get(string name)
        {
            //arrange
            var app = new PokedexTestApplication();
            var client = app.CreateClient();
            var expectedResponse = new PokemonInfoResponse
            {
                Name = name,
                Description = "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.",
                Habitat = "rare",
                IsLegendary = true
            };
            //Act
            var response = await client.GetFromJsonAsync<PokemonInfoResponse>($"/api/pokemon/{name}/basic");

            //Assert
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);
        }


        [Theory]
        [InlineData("mewtwo")]

        public async Task GetTranslated(string name)
        {
            //arrange
            var app = new PokedexTestApplication();
            var client = app.CreateClient();
            var expectedResponse = new PokemonInfoResponse
            {
                Name = name,
                Description = "Created by a scientist after years of horrific gene splicing and dna engineering experiments,  \"it was.\"",
                Habitat = "rare",
                IsLegendary = true
            };

            //Act
            var response = await client.GetFromJsonAsync<PokemonInfoResponse>($"/api/pokemon/{name}/translated");

            //Assert
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);
        }
    }
}