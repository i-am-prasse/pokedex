using System;
using System.Net;

namespace Pokedex.Domain.CustomExceptions
{
    public abstract class HttpValidationException : Exception
	{
		public HttpStatusCode StatusCode { get; private set; }

		protected HttpValidationException(HttpStatusCode statusCode, string content, Exception inner = null) : base(content, inner) => StatusCode = statusCode;
	}
}
