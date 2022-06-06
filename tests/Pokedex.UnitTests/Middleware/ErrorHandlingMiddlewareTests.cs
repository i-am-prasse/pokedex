using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Pokedex.Api.Middleware;
using Pokedex.Domain.CustomExceptions;
using System.Net;

namespace Pokedex.UnitTests.Middleware
{
    public  class ErrorHandlingMiddlewareTests
    {
		[Fact]
		public async Task Invoke_OnExpectedError_ReturnsAppropriateStatusCode()
		{
			//arrange
			var unExpectedException = new NotFoundException("test");
			RequestDelegate mockNextMiddleware = (HttpContext) =>
			{
				return Task.FromException(unExpectedException);
			};
			var httpContext = new DefaultHttpContext();

			var exceptionHandlingMiddleware = new ErrorHandlerMiddleware(mockNextMiddleware);

			//act
			await exceptionHandlingMiddleware.Invoke(httpContext);

			//assert
			((HttpStatusCode)httpContext.Response.StatusCode).Should().Be(HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task Invoke_OnUnexpectedError_Returns500StatusCode()
		{
			//arrange
			var unExpectedException = new ArgumentNullException();
			RequestDelegate mockNextMiddleware = (HttpContext) =>
			{
				return Task.FromException(unExpectedException);
			};
			var httpContext = new DefaultHttpContext();

			var exceptionHandlingMiddleware = new ErrorHandlerMiddleware(mockNextMiddleware);

			//act
			await exceptionHandlingMiddleware.Invoke(httpContext);

			//assert
			((HttpStatusCode)httpContext.Response.StatusCode).Should().Be(HttpStatusCode.InternalServerError);
		}
	}
}
