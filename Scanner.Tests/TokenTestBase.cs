using System.IO;
using Scanner.Names;
using Scanner.Shared;
using Xunit;

namespace Scanner.Tests
{
    public abstract class TokenTestBase<T> where T : IScanner
    {
        public void AssertToken(string input, params Token[] tokens)
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