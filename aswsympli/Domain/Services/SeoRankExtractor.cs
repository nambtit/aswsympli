using Domain.ValueObjects;

namespace Domain.Services
{
    public interface ISeoRankExtractor
    {
        /// <summary>
        /// Find in the search result stream the position where it is listed (i.e. its rank).
        /// The caller owns input stream and is responsible for disposing it. Implementation must NOT dispose it.
        /// </summary>
        public IEnumerable<SeoRecord> Extract(string companyUrl, StreamReader resultStream);
    }
}
