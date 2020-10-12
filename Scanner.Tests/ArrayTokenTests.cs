using Scanner.Arrays;
using Scanner.Shared;
using Xunit;

namespace Scanner.Tests
{
    public class ArrayTokenTests : TokenTestBase<Arrays.Scanner>
    {
        public static readonly Token LsbrToken = new Token((int)ArrayTokenType.Lsbr, "[");
        public static readonly Token RsbrToken = new Token((int)ArrayTokenType.Rsbr, "]");

        [Fact]
        public void ArrayTokenTest_Lsbr()
        {
            AssertToken("[]", LsbrToken, RsbrToken);
        }
    }
}