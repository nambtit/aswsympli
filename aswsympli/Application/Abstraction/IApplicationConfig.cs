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

        /// <summary>
        /// The number of top results to lookup for ranking, e.g. Only check top 50 results.
        /// </summary>
        int TotalSearchResultsToLookup { get; }

        /// <summary>
        /// Relative number of results expected to be in a page. The number of results
        /// can vary depending on search engine behavior, please do not expect it to be always as specified.
        /// Setting too small page size could cause 429 (Too Many Requests) error when the app 
        /// sends many subsequent requests for searches.
        /// </summary>
        int MaxResultPerPage { get; }

        public string DbConnectionString { get; }

        /// <summary>
        /// The expiry in minutes for caching SEO rank data.
        /// </summary>
        public int CacheAbsExpiryMinutes { get; }
    }
}
