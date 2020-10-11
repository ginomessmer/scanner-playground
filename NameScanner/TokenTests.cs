using System.IO;
using Xunit;

namespace NameScanner
{
    public class TokenTests
    {
        public readonly Token AnnaToken = new Token(TokenType.Anna, "anna");
        public readonly Token PeterToken = new Token(TokenType.Peter, "peter");
        public readonly Token PetraToken = new Token(TokenType.Petra, "petra");

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
            var scanner = new Scanner(new StringReader(input));
            Token token;

            foreach (var expected in tokens)
            {
                token = scanner.NextToken();
                Assert.Equal(expected.Type, token.Type);
                Assert.Equal(expected.Text, token.Text);
            }

            token = scanner.NextToken();
            Assert.Equal(TokenType.EOF, token.Type);
        }
    }
}