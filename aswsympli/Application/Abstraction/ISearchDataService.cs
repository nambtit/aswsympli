namespace Application.Abstraction
{
    public interface ISearchDataService
    {
        Task<Stream> GetSearchDataStreamAsync(string keyword, int pageSize, int skip, CancellationToken token);
    }

    public interface IBingSearchDataService : ISearchDataService { }
    public interface IGoogleSearchDataService : ISearchDataService { }
}
