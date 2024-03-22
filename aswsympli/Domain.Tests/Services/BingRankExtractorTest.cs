using System.Reflection;
using CoreUtils.DateTime;
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
            var utcNow = DateTime.UtcNow;
            var mockDateTime = new Mock<IDateTimeService>();
            mockDateTime.Setup(e => e.GetUtcNow()).Returns(utcNow);
            var sut = new BingRankExtractor(mockDateTime.Object);
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
            result.RecordedAtUtc = utcNow;
            stream.EndOfStream.Should().BeTrue();
        }
    }
}
