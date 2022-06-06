using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Pokedex.Api;
using Pokedex.Domain.CustomExceptions;
using Pokedex.Domain.Interfaces;
using Pokedex.Domain.Models;
using Pokedex.Domain.Services;

namespace Pokedex.UnitTests.Services
{
    public class TranslatedPokemonInfoServiceTests
    {
        private readonly Mock<IPokeApiClient> _mockPokeApiClient;
        private readonly Mock<IFunTranslationClient> _mockFunTranslationClient;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<TranslatedPokemonInfoService>> _loggerMock;
        private readonly IFixture _fixture;
        private readonly TranslatedPokemonInfoService _translatedPokemonInfoService;

        public TranslatedPokemonInfoServiceTests()
        {
            _loggerMock = new Mock<ILogger<TranslatedPokemonInfoService>>();
            _mockFunTranslationClient = new Mock<IFunTranslationClient>();
            _mockPokeApiClient = new Mock<IPokeApiClient>();
            _fixture = new Fixture();
            var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));

            _mapper = mapperConfig.CreateMapper();

            _translatedPokemonInfoService = new TranslatedPokemonInfoService(_mockPokeApiClient.Object, _mapper, _mockFunTranslationClient.Object, _loggerMock.Object);
        }

        [Fact]
        public async void GetPokemonInfo_OnCaveHabitat_ReturnsTranslatedYodaDescription()
        {
            //Arrange
            var description = _fixture.Create<string>();
            var basicPokemonInfo = _fixture.Build<BasicPokemonInfoResponse>().With(x => x.Habitat, new Habitat { Name = "cave" }).Create();
            _mockPokeApiClient.Setup(x => x.GetBasicInformation(It.IsAny<string>())).ReturnsAsync(basicPokemonInfo);
            _mockFunTranslationClient.Setup(x => x.GetTranslation(It.IsAny<string>(), "yoda")).ReturnsAsync(description);

            //Act
            var result = await _translatedPokemonInfoService.GetPokemonInfo(_fixture.Create<string>());

            //Assert
            _mockPokeApiClient.Verify(x => x.GetBasicInformation(It.IsAny<string>()), Times.Once());
            result.Description.Should().Be(description);
        }

        [Fact]
        public async void GetPokemonInfo_isLegendary_ReturnsTranslatedYodaDescription()
        {
            //Arrange
            var description = _fixture.Create<string>();
            var basicPokemonInfo = _fixture.Build<BasicPokemonInfoResponse>().With(x => x.IsLegendary, true).Create();
            _mockPokeApiClient.Setup(x => x.GetBasicInformation(It.IsAny<string>())).ReturnsAsync(basicPokemonInfo);
            _mockFunTranslationClient.Setup(x => x.GetTranslation(It.IsAny<string>(), "yoda")).ReturnsAsync(description);

            //Act
            var result = await _translatedPokemonInfoService.GetPokemonInfo(_fixture.Create<string>());

            //Assert
            _mockPokeApiClient.Verify(x => x.GetBasicInformation(It.IsAny<string>()), Times.Once());
            result.Description.Should().Be(description);
        }

        [Fact]
        public async void GetPokemonInfo_NotisLegendaryAndNotCave_ReturnsTranslatedYodaDescription()
        {
            //Arrange
            var description = _fixture.Create<string>();
            var basicPokemonInfo = _fixture.Build<BasicPokemonInfoResponse>().With(x => x.IsLegendary, false).With(x => x.Habitat, new Habitat { Name = "test" }).Create();
            _mockPokeApiClient.Setup(x => x.GetBasicInformation(It.IsAny<string>())).ReturnsAsync(basicPokemonInfo);
            _mockFunTranslationClient.Setup(x => x.GetTranslation(It.IsAny<string>(), "shakespeare")).ReturnsAsync(description);

            //Act
            var result = await _translatedPokemonInfoService.GetPokemonInfo(_fixture.Create<string>());

            //Assert
            _mockPokeApiClient.Verify(x => x.GetBasicInformation(It.IsAny<string>()), Times.Once());
            result.Description.Should().Be(description);
        }

        [Fact]
        public void GetPokemonInfo_OnInValidName_ReturnsNotFoundException()
        {
            //Arrange
            _mockPokeApiClient.Setup(x => x.GetBasicInformation(It.IsAny<string>())).ReturnsAsync(null as BasicPokemonInfoResponse);

            //Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await _translatedPokemonInfoService.GetPokemonInfo(_fixture.Create<string>()));

        }

        [Fact]
        public async void GetPokemonInfo_WhenMultipleDescriptionAvailable_ReturnsFirstDescription()
        {
            //Arrange
            var description = _fixture.Create<string>();
            var basicPokemonInfo = _fixture.Build<BasicPokemonInfoResponse>()
                .With(x => x.IsLegendary, false)
                .With(x => x.Habitat, new Habitat { Name = "test" })
                .With(x => x.FlavorTextEntries, new FlavorTextEntry[] { new FlavorTextEntry() { FlavorText = description, Language = new Language() { Name = "en" } }, new FlavorTextEntry() { FlavorText = "another description", Language = new Language() { Name = "en" } }, new FlavorTextEntry() { FlavorText = "another description 2", Language = new Language() { Name = "in" } } })
                .Create();
            _mockPokeApiClient.Setup(x => x.GetBasicInformation(It.IsAny<string>())).ReturnsAsync(basicPokemonInfo);
            _mockFunTranslationClient.Setup(x => x.GetTranslation(description, "shakespeare")).ReturnsAsync(description); 

            //Act
            var result = await _translatedPokemonInfoService.GetPokemonInfo(description);

            //Assert
            _mockPokeApiClient.Verify(x => x.GetBasicInformation(It.IsAny<string>()), Times.Once());
            result.Description.Should().Be(description);
        }

    }
}
