using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pokedex.Domain.Interfaces;
using Pokedex.Domain.Models;
using System.Text.Json;

namespace Pokedex.Infrastructure.Clients
{
    public class PokeApiClient : IPokeApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PokeApiClient> _logger;
        private readonly IConfiguration _configuration;

        public PokeApiClient(HttpClient httpClient, ILogger<PokeApiClient> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["PokeApiUrl"]);
            _logger = logger;
            
        }

        public async Task<BasicPokemonInfoResponse> GetBasicInformation(string name)
        {
            var basicPokemonInfo = new BasicPokemonInfoResponse();

            try
            {
                var response = await _httpClient.GetAsync($"api/v2/pokemon-species/{name}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                basicPokemonInfo = JsonSerializer.Deserialize<BasicPokemonInfoResponse>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while calling the PokeAPI service. Exception message - {ex.Message} ");
                return null;
            }

            return basicPokemonInfo;
        }
    }
}
