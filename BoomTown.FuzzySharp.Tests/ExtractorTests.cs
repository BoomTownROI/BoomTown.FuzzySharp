using System.Linq;
using Xunit;

namespace BoomTown.FuzzySharp.Tests
{
    public class ExtractorTests
    {
        private readonly string[] _choices;
        private Extractor _extractor;

        public ExtractorTests()
        {
            _extractor = new Extractor();
            _choices = new[] {"google", "bing", "facebook", "linkedin", "twitter", "googleplus", "bingnews", "plexoogl"};
        }

        [Theory]
        [InlineData("goolge", 16, 7)]
        [InlineData("bling", 50, 3)]
        [InlineData("words", 0, 8)]
        public void TestExtractWithoutOrder(string value, int cutoff, int expectedCount)
        {
            _extractor = new Extractor(cutoff);
            
            var results = _extractor.ExtractWithoutOrder(value, _choices).ToList();
            
            Assert.Equal(expectedCount, results.Count);
            Assert.True(results.All(x => x.Score >= cutoff));
        }

        [Theory]
        [InlineData("goolge", "google")]
        [InlineData("bling", "bing")]
        [InlineData("tweet", "twitter")]
        [InlineData("books", "facebook")]
        public void TestExtractBest(string value, string expected)
        {
            var result = _extractor.ExtractBest(value, _choices);

            Assert.Equal(result.Value, expected);
        }

        [Theory]
        [InlineData("goolge", 3)]
        [InlineData("goolge", 4)]
        [InlineData("words", 1)]
        public void TestExtractTop(string value, int limit)
        {
            var result = _extractor.ExtractTop(value, _choices, limit).ToList();

            Assert.Equal(limit, result.Count);
        }
    }
}