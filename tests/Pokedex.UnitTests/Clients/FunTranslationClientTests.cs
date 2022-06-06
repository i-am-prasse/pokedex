using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Pokedex.Infrastructure.Clients;
using System.Net;

namespace Pokedex.UnitTests.Clients
{
    public class FunTranslationClientTests
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<ILogger<FunTranslationClient>> _mockLogger;
        private FunTranslationClient _funTranslationClient;
        private readonly IFixture _fixture;

        public FunTranslationClientTests()
        {
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.SetupGet(x => x[It.Is<string>(s => s == "FunTranslatorUrl")]).Returns("https://testapi.test.com/");
            _mockLogger = new Mock<ILogger<FunTranslationClient>>();
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetTranslation_OnSuccess_ShouldReturnTranslatedDescription()
        {
            //Arrange
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{""success"": {""total"": 1},""contents"": {""translated"": ""Mr, you gave.Tim a hearty meal,Made him die,  but unfortunately what he ate."",
                                                ""text"": ""You gave Mr. Tim a hearty meal, but unfortunately what he ate made him die."", ""translation"": ""yoda""}}")};

            _funTranslationClient = new FunTranslationClient(TestHelper.GetMockHttpClient(mockResponse), _mockLogger.Object, _mockConfig.Object);

            // Act
            var response = await _funTranslationClient.GetTranslation(_fixture.Create<string>(), _fixture.Create<string>());

            // Assert
            response.Should().NotBeEmpty();
            response.Should().Be("Mr, you gave.Tim a hearty meal,Made him die,  but unfortunately what he ate.");

        }

        [Fact]
        public async Task GetTranslation_OnUnsuccessfulHttpRequest_ShouldReturnOriginalDescription()
        {
            //Arrange
            var description = _fixture.Create<string>();
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            };

            _funTranslationClient = new FunTranslationClient(TestHelper.GetMockHttpClient(mockResponse), _mockLogger.Object, _mockConfig.Object);

            // Act
            var response = await _funTranslationClient.GetTranslation(description, _fixture.Create<string>());

            // Assert
            response.Should().NotBeEmpty();
            response.Should().Be(description);

        }
    }
}
