using Scanner.Names;
using Scanner.Shared;
using System.IO;
using Xunit;

namespace Scanner.Tests
{
    public class NameTokenTests
    {
        public readonly Token AnnaToken = new Token((int)NameTokenType.Anna, "anna");
        public readonly Token PeterToken = new Token((int)NameTokenType.Peter, "peter");
        public readonly Token PetraToken = new Token((int)NameTokenType.Petra, "petra");

        [Fact]
        public void TokenTest_Peter()
        {
            AssertToken("peter", PeterToken);
            AssertToken(" peter peter", PeterToken, PeterToken);
            AssertToken("peter   peter peter", PeterToken, PeterToken, PeterToken);
            AssertToken("peterpeter", PeterToken, PeterToken);
        }

        [Fact]
        public void TokenTest_Anna()
        {
            AssertToken("anna", AnnaToken);
            AssertToken("anna anna ", AnnaToken, AnnaToken);
            AssertToken("annaanna", AnnaToken, AnnaToken);
        }

        [Fact]
        public void TokenTest_Petra()
        {
            AssertToken("petra", PetraToken);
            AssertToken("petrapetra", PetraToken, PetraToken);
            AssertToken("petra petra   petra", PetraToken, PetraToken, PetraToken);
        }

        [Fact]
        public void TokenTest_Mix()
        {
            AssertToken("anna peter petra", AnnaToken, PeterToken, PetraToken);
            AssertToken("anna anna petra peter", AnnaToken, AnnaToken, PetraToken, PeterToken);
        }

        static void AssertToken(string input, params Token[] tokens)
        {
            var scanner = new Names.Scanner(new StringReader(input));
            Token token;

            foreach (var expected in tokens)
            {
                token = scanner.NextToken();
                Assert.Equal(expected.Type, token.Type);
                Assert.Equal(expected.Text, token.Text);
            }

            token = scanner.NextToken();
            Assert.Equal((int)NameTokenType.EOF, token.Type);
        }
    }
}