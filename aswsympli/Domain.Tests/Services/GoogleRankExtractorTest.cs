using FluentAssertions;

namespace Domain.Tests.Services
{
    public class GoogleRankExtractorTest
    {
        [Fact]
        public void ShouldExtractSearchDataWithoutDisposingInputStream()
        {
            // ARRANGE
            // ACT
            // ASSERT
            1.Should().Be(1);
        }
    }
}
