using System.Text.Json;

namespace Pokedex.Api.Dto.Response
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public List<ErrorResponseItem> Errors { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }

    public class ErrorResponseItem
    {
        public string Field { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public override string ToString() => $"{Field}: {string.Join(",", Errors)}";
    }
}
