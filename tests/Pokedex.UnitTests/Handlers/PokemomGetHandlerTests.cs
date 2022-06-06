using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Pokedex.Api;
using Pokedex.Api.Dto.Request;
using Pokedex.Api.Handlers;
using Pokedex.Domain;
using Pokedex.Domain.Factories;
using Pokedex.Domain.Models;
using Pokedex.Domain.Services;

namespace Pokedex.UnitTests.Handlers
{
    public class PokemomGetHandlerTests
    {
        private readonly Mock<ILogger<PokemonGetHandler>> _loggerMock;
        private readonly Mock<IPokemonInfoServiceFactory> _pokemonInfoServiceFactoryMock;
        private readonly Mock<IPokemonInfoService> _pokemonInfoServiceMock;
        private readonly PokemonGetHandler _pokemonGetHandler;
        private readonly IMapper _mapper;
        private readonly IFixture _fixture;

        public PokemomGetHandlerTests()
        {
            _loggerMock = new Mock<ILogger<PokemonGetHandler>>();
            _pokemonInfoServiceFactoryMock = new Mock<IPokemonInfoServiceFactory>();
            _fixture = new Fixture();
            var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));

            _mapper = mapperConfig.CreateMapper();
            _pokemonInfoServiceMock = new Mock<IPokemonInfoService>();
            _pokemonGetHandler = new PokemonGetHandler(_pokemonInfoServiceFactoryMock.Object,_mapper);
        }

        [Fact]
        public async Task Handle_WithPokemonfound_ExpectsSuccessAndReturnsPokemonInfo()
        {
            //Arrange
            var request = _fixture.Build<GetPokemonRequest>().With(x => x.Name).Create();
            var pokemonInfo = _fixture.Create<PokemonInfo>();
            _pokemonInfoServiceMock.Setup(x => x.GetPokemonInfo(It.IsAny<string>())).ReturnsAsync(pokemonInfo);
            _pokemonInfoServiceFactoryMock.Setup(c => c.Create(It.IsAny<PokemonInfoType>())).Returns(_pokemonInfoServiceMock.Object);

            //Act
            var result = await _pokemonGetHandler.Handle(request, CancellationToken.None);

            //Assert
            _pokemonInfoServiceMock.Verify(p => p.GetPokemonInfo(It.IsAny<string>()), Times.Once);
            result.Name.Should().Be(pokemonInfo.Name);
        }

        [Fact]
        public async Task HandleAsync_WithPokemonNotFound_ThrowsException()
        {
            //Arrange
            var request = _fixture.Build<GetPokemonRequest>().With(x => x.Name).Create();
            var pokemonInfo = _fixture.Create<PokemonInfo>();
            _pokemonInfoServiceMock.Setup(x => x.GetPokemonInfo(It.IsAny<string>())).ReturnsAsync(pokemonInfo);
            _pokemonInfoServiceFactoryMock.Setup(c => c.Create(It.IsAny<PokemonInfoType>())).Throws<Exception>();

            //Act
            var result = new Func<Task>(async () => await _pokemonGetHandler.Handle(request, CancellationToken.None));

            //Assert
            await result.Should().ThrowAsync<Exception>();

        }

    }
}
