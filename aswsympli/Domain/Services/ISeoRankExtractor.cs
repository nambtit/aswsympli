using Domain.ValueObjects;

namespace Domain.Services
{
    public interface ISEORankExtractor
    {
        /// <summary>
        /// Find in the search result stream the position where the company is listed (i.e. its rank).
        /// The caller owns input stream and is responsible for disposing it, implementations of this must NOT dispose it.
        /// </summary>
        public RankExtractResult Extract(string companyUrl, StreamReader resultStream);
    }

    public interface IGoogleSEORankExtractor : ISEORankExtractor { }
    public interface IBingSEORankExtractor : ISEORankExtractor { }
}
