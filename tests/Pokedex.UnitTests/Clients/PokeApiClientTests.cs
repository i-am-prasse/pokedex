using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Pokedex.Infrastructure.Clients;
using System.Net;

namespace Pokedex.UnitTests.Clients
{
    public class PokeApiClientTests
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<ILogger<PokeApiClient>> _mockLogger;
        private PokeApiClient _pokeApiClient;
        private readonly IFixture _fixture;

        public PokeApiClientTests()
        {
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.SetupGet(x => x[It.Is<string>(s => s == "PokeApiUrl")]).Returns("https://testapi.test.com/");
            _mockLogger = new Mock<ILogger<PokeApiClient>>();
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetBasicInformation_OnSuccess_ShouldReturnBasicInformation()
        {
            //Arrange
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{ ""name"": ""pokedex""}"),
            };

            _pokeApiClient = new PokeApiClient(TestHelper.GetMockHttpClient(mockResponse), _mockLogger.Object, _mockConfig.Object);

            // Act
            var response = await _pokeApiClient.GetBasicInformation(_fixture.Create<string>());

            // Assert
            response.Name.Should().NotBeEmpty();
            response.Name.Should().Be("pokedex");

        }

        [Fact]
        public async Task GetBasicInformation_OnUnsuccessfulHttpRequest_ShouldNotThrowExceptionButReturnNull()
        {
            //Arrange
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            };

            _pokeApiClient = new PokeApiClient(TestHelper.GetMockHttpClient(mockResponse), _mockLogger.Object, _mockConfig.Object);

            // Act
            var response = await _pokeApiClient.GetBasicInformation(_fixture.Create<string>());

            // Assert
            response.Should().Be(null);
        }
    }
}
