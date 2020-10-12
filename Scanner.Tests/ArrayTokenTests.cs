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
        public static readonly Token NumberToken = new Token((int)ArrayTokenType.Number, "1");
        public static readonly Token NullToken = new Token((int)ArrayTokenType.Null, "null");

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

        [Fact]
        public void ArrayTokenTest_LsbrNameNumberNameRsbr()
        {
            AssertToken("[name,1,name]",
                LsbrToken,
                NameToken,
                CommaToken,
                NumberToken,
                CommaToken,
                NameToken,
                RsbrToken);
        }

        [Fact]
        public void ArrayTokenTest_LsbrNameNullNameRsbr()
        {
            AssertToken("[name,null,name]",
                LsbrToken,
                NameToken,
                CommaToken,
                NullToken,
                CommaToken,
                NameToken,
                RsbrToken);
        }

        [Fact]
        public void ArrayTokenTest_LsbrNameNullNumberNameRsbr()
        {
            AssertToken("[name,null,1,name]",
                LsbrToken,
                NameToken,
                CommaToken,
                NullToken,
                CommaToken,
                NumberToken,
                CommaToken,
                NameToken,
                RsbrToken);
        }
    }
}