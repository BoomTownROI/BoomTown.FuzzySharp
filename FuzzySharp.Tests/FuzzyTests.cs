using Xunit;

namespace FuzzySharp.Tests
{
    public class FuzzySharpTests
    {
        [Theory]
        [InlineData("mysmilarstring", "mymostsimilarstsdsdring", 76)]
        [InlineData("mysmilarstring", "myawfullysimilarstirng", 72)]
        [InlineData("mysmilarstring", "mysimilarstring", 97)]
        [InlineData("csr", "c s r", 75)]
        [InlineData("  SeAFood Is ThE bEsT FOod  ", "seafood is tHe best foOd", 100)]
        public void TestRatio(string s1, string s2, int expected)
        {
            Assert.Equal(expected, Fuzzy.Ratio(s1, s2));
        }

        [Theory]
        [InlineData("similar", "somewhresimlrbetweenthisstring", 71)]
        [InlineData("similar", "notinheresim", 43)]
        [InlineData("pros holdings, inc.", "settlement facility dow corning trust", 38)]
        [InlineData("Should be the same", "Opposite ways go alike", 33)]
        [InlineData("Opposite ways go alike", "Should be the same", 33)]
        public void TestCaseSensitivePartialRatio(string s1, string s2, int expected)
        {
            Assert.Equal(expected, Fuzzy.PartialRatio(s1, s2, StringOptions.CaseSensitive, StringOptions.PreserveNonAlphaNumeric));
        }

        [Fact]
        public void TestSpecialCharacterRatio()
        {
            Assert.Equal(96, Fuzzy.TokenSetRatio("Steve Ordonez", "Steve Ordoí±ez"));
        }
        
        [Theory]
        [InlineData("mvn", "wwwwww.mavencentral.comm", 67)]
        [InlineData("  order words out of ", "  words out of order", 100)]
        [InlineData("Testing token set ratio token", "Added another test", 44)]
        public void TestTokenSortPartial(string s1, string s2, int expected)
        {
            Assert.Equal(expected, Fuzzy.TokenSortPartialRatio(s1, s2));
        }

        [Theory]
        [InlineData("FUZZY was a bear", "fuzzy fuzzy was a bear", 84)]
        public void TestTokenSortRatio(string s1, string s2, int expected)
        {
            Assert.Equal(expected, Fuzzy.TokenSortRatio(s1, s2));
        }
        
        [Theory]
        [InlineData("fuzzy fuzzy fuzzy bear", "fuzzy was a bear", 100)]
        [InlineData("Testing token set ratio token", "Added another test", 39)]
        public void TestTokenSetRatio(string s1, string s2, int expected)
        {
            Assert.Equal(expected, Fuzzy.TokenSetRatio(s1, s2));
        }
        
        [Theory]
        [InlineData("fuzzy was a bear", "blind 100", 11)]
        [InlineData("chicago transit authority", "cta", 67)]
        [InlineData("steve p", "steven perry", 86)]
        public void TestTokenSetPartialRatio(string s1, string s2, int expected)
        {
            Assert.Equal(expected, Fuzzy.TokenSetPartialRatio(s1, s2));
        }

        [Theory]
        [InlineData("mvn", "wwwwww.mavencentral.comm", 60)]
        [InlineData("mvn", "www;'l3;4;.4;23.4/23.4/234//////www.mavencentral.comm", 40)]
        [InlineData("The quick brown fox jimps ofver the small lazy dog", "the quick brown fox jumps over the small lazy dog", 97)]
        public void TestWeightedRatio(string s1, string s2, int expected)
        {
            Assert.Equal(expected, Fuzzy.WeightedRatio(s1, s2));
        }
    }
}