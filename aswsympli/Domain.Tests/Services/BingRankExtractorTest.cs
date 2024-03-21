using System.Reflection;
using Domain.Services;

namespace Domain.Tests.Services
{
	public class BingRankExtractorTest
	{
		public BingRankExtractorTest()
		{
		}

		[Fact]
		public void ShouldExtractSearchDataProperlyFromBing()
		{
			// Arrange
			var sut = new BingRankExtractor();
			var currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var dataFile = Path.Combine(currentDir, @"Data/Bing-Top10-[Ranked-0-1].txt");
			using var stream = new StreamReader(dataFile);
			const string companyUrl = "https://www.dummysite.test";

			// Act
			var result = sut.Extract(companyUrl, stream);

			// Assert
			result.Should().NotBeNull();
			result.TotalResults.Should().Be(17);
			result.Engine.Should().Be(Enums.SearchEngineEnum.Bing);
			result.Ranks.Should().Equal(new[] { 0, 1 }.Order());
			stream.EndOfStream.Should().BeTrue();
		}
	}
}
