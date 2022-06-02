using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pokedex.Api.Controllers;

namespace Pokedex.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            using var logFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = logFactory.CreateLogger<WeatherForecastController>();
            var controller = new WeatherForecastController(logger);

            // Act
            var result = controller.Get();

            // Assert
            var viewResult = Assert.IsType<Pokedex.Api.WeatherForecast[]>(result);
        }
    }
}