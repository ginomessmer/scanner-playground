using System;
using System.IO;
using Scanner.Shared;
using Xunit;

namespace Scanner.Tests
{
    public abstract class TokenTestBase<TScanner> where TScanner : BaseScanner
    {
        public void AssertToken(string input, params Token[] tokens)
        {
            var scanner = Activator.CreateInstance(typeof(TScanner), new StringReader(input)) as TScanner;
            Token token;

            foreach (var expected in tokens)
            {
                token = scanner.NextToken();
                Assert.Equal(expected.Type, token.Type);
                Assert.Equal(expected.Text, token.Text);
            }

            token = scanner.NextToken();
            Assert.Equal(0, token.Type);
        }
    }
}