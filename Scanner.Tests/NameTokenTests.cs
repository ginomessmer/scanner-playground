using Scanner.Languages.Names;
using Scanner.Shared;
using Xunit;

namespace Scanner.Tests
{
    public class NameTokenTests : TokenTestBase<Languages.Names.Scanner>
    {
        public static readonly Token AnnaToken = new Token((int)NameTokenType.Anna, "anna");
        public static readonly Token PeterToken = new Token((int)NameTokenType.Peter, "peter");
        public static readonly Token PetraToken = new Token((int)NameTokenType.Petra, "petra");

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
    }
}