namespace Application.Abstraction
{
    public interface ISearchDataService
    {
        Task<Stream> GetSearchDataStreamAsync(string keyword);
    }

    public interface IBingSearchDataService : ISearchDataService { }
    public interface IGoogleSearchDataService : ISearchDataService { }
}
