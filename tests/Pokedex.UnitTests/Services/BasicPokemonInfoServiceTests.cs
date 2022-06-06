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
    public class BasicPokemonInfoServiceTests
    {
        private readonly Mock<IPokeApiClient> _mockPokeApiClient;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<BasicPokemonInfoService>> _loggerMock;
        private readonly IFixture _fixture;
        private readonly BasicPokemonInfoService _basicPokemonInfoService;

        public BasicPokemonInfoServiceTests()
        {
            _loggerMock = new Mock<ILogger<BasicPokemonInfoService>>();
            _mockPokeApiClient = new Mock<IPokeApiClient>();
            _fixture = new Fixture();
            var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));

            _mapper = mapperConfig.CreateMapper();
            _basicPokemonInfoService = new BasicPokemonInfoService(_mockPokeApiClient.Object,_mapper,_loggerMock.Object);
        }

        [Fact]
        public async void GetPokemonInfo_OnValidName_ReturnsBasicInformation()
        {
            //Arrange
            var basicPokemonInfo = _fixture.Create<BasicPokemonInfoResponse>();
            _mockPokeApiClient.Setup(x=>x.GetBasicInformation(It.IsAny<string>())).ReturnsAsync(basicPokemonInfo);

            //Act
            var result = await _basicPokemonInfoService.GetPokemonInfo(_fixture.Create<string>());

            //Assert
            _mockPokeApiClient.Verify(x=>x.GetBasicInformation(It.IsAny<string>()), Times.Once());
            result.Name.Should().Be(basicPokemonInfo.Name);
        }

        [Fact]
        public void GetPokemonInfo_OnInValidName_ReturnsNotFoundException()
        {
            //Arrange
            _mockPokeApiClient.Setup(x => x.GetBasicInformation(It.IsAny<string>())).ReturnsAsync((BasicPokemonInfoResponse)null);

            //Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await _basicPokemonInfoService.GetPokemonInfo(_fixture.Create<string>()));

        }
    }
}
