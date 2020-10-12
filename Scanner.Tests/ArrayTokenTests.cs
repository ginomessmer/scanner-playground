using Scanner.Arrays;
using Scanner.Shared;
using Xunit;

namespace Scanner.Tests
{
    public class ArrayTokenTests : TokenTestBase<Arrays.Scanner>
    {
        public static readonly Token LsbrToken = new Token((int)ArrayTokenType.Lsbr, "[");
        public static readonly Token RsbrToken = new Token((int)ArrayTokenType.Rsbr, "]");
        public static readonly Token NameToken = new Token((int)ArrayTokenType.Name, "name");
        public static readonly Token CommaToken = new Token((int)ArrayTokenType.Comma, ",");

        [Fact]
        public void ArrayTokenTest_LsbrRsbr()
        {
            AssertToken("[]", LsbrToken, RsbrToken);
        }

        [Fact]
        public void ArrayTokenTest_LsbrNameRsbr()
        {
            AssertToken("[name]", LsbrToken, NameToken, RsbrToken);
        }

        [Fact]
        public void ArrayTokenTest_LsbrNameNameRsbr()
        {
            AssertToken("[name,name]", LsbrToken, NameToken, CommaToken, NameToken, RsbrToken);
        }
    }
}