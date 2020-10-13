using Scanner.Languages.ArraysWithFractions;
using Scanner.Shared;
using Xunit;

namespace Scanner.Tests
{
    public class ArrayWithFractionsTests : TokenTestBase<Languages.ArraysWithFractions.Scanner>
    {
        public static readonly Token LsbrToken = new Token((int)ArrayTokenType.Lsbr, "[");
        public static readonly Token RsbrToken = new Token((int)ArrayTokenType.Rsbr, "]");
        public static readonly Token NameToken = new Token((int)ArrayTokenType.Name, "name");
        public static readonly Token CommaToken = new Token((int)ArrayTokenType.Comma, ",");
        public static readonly Token NumberToken = new Token((int)ArrayTokenType.Number, "1");
        public static readonly Token NullToken = new Token((int)ArrayTokenType.Null, "null");
        public static readonly Token FractionToken = new Token((int)ArrayTokenType.Fraction, "1.1");

        [Fact]
        public void ArrayWithFractionsTokenTest_LsbrRsbr()
        {
            AssertToken("[]", LsbrToken, RsbrToken);
        }

        [Fact]
        public void ArrayWithFractionsTokenTest_LsbrNameRsbr()
        {
            AssertToken("[name]", LsbrToken, NameToken, RsbrToken);
        }

        [Fact]
        public void ArrayWithFractionsTokenTest_LsbrNameNameRsbr()
        {
            AssertToken("[name,name]", LsbrToken, NameToken, CommaToken, NameToken, RsbrToken);
        }

        [Fact]
        public void ArrayWithFractionsTokenTest_LsbrNameNumberNameRsbr()
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
        public void ArrayWithFractionsTokenTest_LsbrNameFractionNameRsbr()
        {
            AssertToken("[name,1.1,name]",
                LsbrToken,
                NameToken,
                CommaToken,
                FractionToken,
                CommaToken,
                NameToken,
                RsbrToken);
        }

        [Fact]
        public void ArrayWithFractionsTokenTest_LsbrNameFractionExpNameRsbr()
        {
            AssertToken("[name,1.1^1,name]",
                LsbrToken,
                NameToken,
                CommaToken,
                FractionToken.SetText("1.1^1"),
                CommaToken,
                NameToken,
                RsbrToken);
        }

        [Fact]
        public void ArrayWithFractionsTokenTest_LsbrNameFractionExpNameRsbr_Complex()
        {
            AssertToken("[name,123.123^123,name]",
                LsbrToken,
                NameToken,
                CommaToken,
                FractionToken.SetText("123.123^123"),
                CommaToken,
                NameToken,
                RsbrToken);
        }

        [Fact]
        public void ArrayWithFractionsTokenTest_LsbrNameFractionExpNameRsbr_WithoutDecimalPlace()
        {
            AssertToken("[name,1.^1,name]",
                LsbrToken,
                NameToken,
                CommaToken,
                FractionToken.SetText("1.^1"),
                CommaToken,
                NameToken,
                RsbrToken);
        }

        [Fact]
        public void ArrayWithFractionsTokenTest_LsbrNameFractionExpNameRsbr_WithDoubleDigitPreDecimalPointPosition()
        {
            AssertToken("[name,11.^1,name]",
                LsbrToken,
                NameToken,
                CommaToken,
                FractionToken.SetText("11.^1"),
                CommaToken,
                NameToken,
                RsbrToken);
        }

        [Fact]
        public void ArrayWithFractionsTokenTest_LsbrNameFractionExpNameRsbr_ExpComplex()
        {
            AssertToken("[name,123.^123,name]",
                LsbrToken,
                NameToken,
                CommaToken,
                FractionToken.SetText("123.^123"),
                CommaToken,
                NameToken,
                RsbrToken);
        }
    }
}