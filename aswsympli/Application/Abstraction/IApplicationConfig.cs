namespace Application.Abstraction
{
    public interface IApplicationConfig
    {
        /// <summary>
        /// The list of keywords to be searched in different engine.
        /// </summary>
        IEnumerable<string> SearchKeywords { get; }

        /// <summary>
        /// The URL of the company which will be checked for rank in the search result.
        /// </summary>
        string CompanyUrl { get; }

        /// <summary>
        /// How frequent the background task should call to update SEO rank data, in minutes.
        /// </summary>
        int SEORankDataRefreshFrequencyMinutes { get; }
    }
}
