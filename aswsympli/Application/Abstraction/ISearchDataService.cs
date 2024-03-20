using Domain.Enums;

namespace Application.Abstraction
{
    public interface ISearchDataService
    {
        Task<Stream> GetSearchDataStreamAsync(SearchEngineEnum searchEngine, string keyword);

        Task<Stream> GetSearchDataStreamAsync(string keyword);
    }
}
