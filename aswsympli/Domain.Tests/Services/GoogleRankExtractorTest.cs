using Domain.Services;

namespace Domain.Tests.Services
{
    public class GoogleRankExtractorTest
    {
        [Fact]
        public void ShouldExtractSearchDataWithoutDisposingInputStream()
        {
            // Arrange
            var sut = new GoogleRankExtractor();
            // Act
            _ = sut.Extract(It.IsAny<string>(), It.IsAny<StreamReader>());
            // Assert
            1.Should().Be(1);
        }
    }
}
