using System.Reflection;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Api;
using Pokedex.Api.Dto.Response;
using Pokedex.Api.Middleware;
using Pokedex.Api.Validators;
using Pokedex.Domain;
using Pokedex.Domain.Factories;
using Pokedex.Domain.Interfaces;
using Pokedex.Domain.Services;
using Pokedex.Infrastructure.Clients;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddFluentValidation(x => { x.RegisterValidatorsFromAssemblyContaining<GetPokemonRequestValidator>(); });

builder.Services.Configure((Action<ApiBehaviorOptions>)(options =>
{
    options.InvalidModelStateResponseFactory = c =>
    {
        var errors = c.ModelState.Where(v => v.Value.Errors.Count > 0)
                              .Select(v => new ErrorResponseItem { Field = v.Key, Errors = v.Value.Errors.Select(e => e.ErrorMessage) })
                              .ToList();

        LogBadRequestWarning(c, errors);

        return new BadRequestObjectResult(new ErrorResponse
        {
            StatusCode = 400,
            Message = "One or more Validation errors Occurred",
            Errors = errors
        });
    };
}));

var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));

var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddHttpClient<IPokeApiClient, PokeApiClient>()
              .SetHandlerLifetime(TimeSpan.FromMinutes(5))
              .AddPolicyHandler(Pokedex.Api.Helpers.PollyHelper.GetTransientRetryWithCircuitBreakerPolicy());

builder.Services.AddHttpClient<IFunTranslationClient, FunTranslationClient>()
              .SetHandlerLifetime(TimeSpan.FromMinutes(5))
              .AddPolicyHandler(Pokedex.Api.Helpers.PollyHelper.GetTransientRetryWithCircuitBreakerPolicy());

builder.Services.AddScoped<BasicPokemonInfoService>();
builder.Services.AddScoped<TranslatedPokemonInfoService>();

builder.Services.AddScoped<IPokemonInfoServiceFactory>(ctx =>
{
    var pokemonInfoServices = new Dictionary<PokemonInfoType, Func<IPokemonInfoService>>()
    {
        [PokemonInfoType.BASIC] = () => ctx.GetService<BasicPokemonInfoService>(),
        [PokemonInfoType.TRANSLATED] = () => ctx.GetService<TranslatedPokemonInfoService>(),
    };
    return new PokemonInfoServiceFactory(pokemonInfoServices);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.MapHealthChecks("/healthcheck");

app.Run();


static void LogBadRequestWarning(ActionContext context, List<ErrorResponseItem> errors)
{
    var loggerFactory = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger(context.ActionDescriptor.DisplayName);

    var request = context.HttpContext.Request;

    logger.LogWarning(@$"Automatic Bad Request occurred.
                                        {Environment.NewLine} Error(s): {string.Join(Environment.NewLine, errors)}
                                        {Environment.NewLine}|{request.Method}| Full URL: {request.Path}{request.QueryString}");
}

public partial class Program { }
