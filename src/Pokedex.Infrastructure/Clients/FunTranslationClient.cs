using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pokedex.Domain.Interfaces;
using Pokedex.Domain.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Pokedex.Infrastructure.Clients
{
    public class FunTranslationClient : IFunTranslationClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FunTranslationClient> _logger;
        private readonly IConfiguration _configuration;

        public FunTranslationClient(HttpClient httpClient, ILogger<FunTranslationClient> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["FunTranslatorUrl"]);
            _logger = logger;
        }

        public async Task<string> GetTranslation(string request, string type)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/translate/{type}?text=\"{Regex.Replace(request, @"\t|\n|\r|\f", " ")}\"");

                response.EnsureSuccessStatusCode();

                var translationResponse = JsonSerializer.Deserialize<TranslationResponse>(await response.Content.ReadAsStringAsync());

                if (translationResponse?.Success?.Total > 0 && !string.IsNullOrEmpty(translationResponse?.Contents?.Translated))
                    return translationResponse.Contents.Translated;
                else
                    return request;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while calling the translation service. Exception message - {ex.Message} ");
                return request;
            }
        }
    }
}
