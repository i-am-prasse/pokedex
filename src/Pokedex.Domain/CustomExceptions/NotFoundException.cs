using System;
using System.Net;

namespace Pokedex.Domain.CustomExceptions
{
	public class NotFoundException : HttpValidationException
	{
		public NotFoundException(string content, Exception inner = null) : base(HttpStatusCode.NotFound, content, inner)
		{
		}
	}
}
