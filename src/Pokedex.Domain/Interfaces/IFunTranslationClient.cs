namespace Pokedex.Domain.Interfaces
{
    public interface IFunTranslationClient
    {
        Task<string> GetTranslation(string request, string type);
    }
}