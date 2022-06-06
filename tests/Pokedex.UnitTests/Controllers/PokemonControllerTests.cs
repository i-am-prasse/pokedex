using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Pokedex.Api.Controllers;
using Pokedex.Api.Dto.Request;
using Pokedex.Api.Dto.Response;
using Pokedex.Domain.CustomExceptions;

namespace Pokedex.UnitTests.Controllers
{
    public class PokemonControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PokemonController _controller;
        private readonly Mock<ILogger<PokemonController>> _mockLogger;

        public PokemonControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<PokemonController>>();
            _controller = new PokemonController(_mediatorMock.Object, _mockLogger.Object);
        }

        [Theory]
        [InlineData("mewtwo")]
        public async Task Get_ValidPokemonName_ReturnsBasicInformation(string name)
        {
            //arrange
            var pokemonInfo = new PokemonInfoResponse
            {
                Name = name,
                Description = "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.",
                Habitat = "rare",
                IsLegendary = true
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPokemonRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(pokemonInfo);

            //act
            var result = await _controller.Get(name);

            //assert
            result.Should().NotBeNull();
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetPokemonRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            var okRes = (OkObjectResult)result;

            okRes.Value.Should().BeEquivalentTo(pokemonInfo);

        }

        [Theory]
        [InlineData("xxx")]
        public async Task Get_OnNotFoundException_ReturnsNotFoundObjectResult(string name)
        {
            //arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPokemonRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new NotFoundException("test error message"));

            //act
            var result = await _controller.Get(name);

            //assert
            result.Should().NotBeNull();
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetPokemonRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Theory]
        [InlineData("mewtwo")]
        public async Task GetTranslated_ValidPokemonName_ReturnsBasicInformation(string name)
        {
            //arrange
            var pokemonInfo = new PokemonInfoResponse
            {
                Name = name,
                Description = "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.",
                Habitat = "rare",
                IsLegendary = true
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPokemonRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(pokemonInfo);

            //act
            var result = await _controller.GetTranslated(name);

            //assert
            result.Should().NotBeNull();
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetPokemonRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            var okRes = (OkObjectResult)result;

            okRes.Value.Should().BeEquivalentTo(pokemonInfo);

        }

        [Theory]
        [InlineData("xxx")]
        public async Task GetTranslated_OnNotFoundException_ReturnsNotFoundObjectResult(string name)
        {
            //arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPokemonRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new NotFoundException("test error message"));

            //act
            var result = await _controller.GetTranslated(name);

            //assert
            result.Should().NotBeNull();
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetPokemonRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
