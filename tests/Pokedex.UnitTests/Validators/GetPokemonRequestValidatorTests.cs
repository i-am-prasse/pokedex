using FluentAssertions;
using Pokedex.Api.Dto.Request;
using Pokedex.Api.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.UnitTests.Validators
{
    public class GetPokemonRequestValidatorTests
    {
        private readonly GetPokemonRequestValidator _requestValidator;

        public GetPokemonRequestValidatorTests()
        {
            _requestValidator = new GetPokemonRequestValidator();
        }

		[Fact]
		public void Validate_WithNullName_ReturnsValidationError()
		{
            //Arrange
            var request = new GetPokemonRequest
            {
                Name = null
            };

            //Act
            var result = _requestValidator.Validate(request);

            //Assert
            result.IsValid.Should().BeFalse();
		}

		[Theory]
		[InlineData("aaa")]
		[InlineData("bbb")]
		public void Validate_WithValidName_ReturnsNoValidationError(string name)
		{
            //Arrange
            var request = new GetPokemonRequest
            {
                Name = name
            };

            //Act
            var result = _requestValidator.Validate(request);

			//Assert
			result.IsValid.Should().BeTrue();
		}
	}
}
