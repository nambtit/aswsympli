﻿using System.Reflection;
using Domain.Services;

namespace Domain.Tests.Services
{
	public class GoogleRankExtractorTest
	{
		public GoogleRankExtractorTest()
		{
		}

		[Fact]
		public void ShouldExtractSearchDataProperlyFromGoogle()
		{
			// Arrange
			var sut = new GoogleRankExtractor();
			var currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var dataFile = Path.Combine(currentDir, @"Data/Google-Top10-[Ranked-1-6].txt");
			using var stream = new StreamReader(dataFile);
			const string companyUrl = "https://www.dummysite.test";

			// Act
			var result = sut.Extract(companyUrl, stream);

			// Assert
			result.Should().NotBeNull();
			result.TotalResults.Should().Be(10);
			result.Engine.Should().Be(Enums.SearchEngineEnum.Google);
			result.Ranks.Should().Equal(new[] { 1, 6 }.Order());
			stream.EndOfStream.Should().BeTrue();
		}
	}
}
