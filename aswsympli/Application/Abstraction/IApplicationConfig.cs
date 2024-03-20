namespace Application.Abstraction
{
    public interface IApplicationConfig
    {
        IEnumerable<string> SearchKeywords { get; }

        string CompanyUrl { get; }
    }
}
