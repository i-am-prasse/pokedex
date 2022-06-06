using FluentValidation;
using Pokedex.Api.Dto.Request;

namespace Pokedex.Api.Validators
{
    public class GetPokemonRequestValidator : AbstractValidator<GetPokemonRequest>
    {
		public GetPokemonRequestValidator() 
		{
			RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name must be specified");

			RuleFor(x => x.Type).IsInEnum();
		}
	}
}
