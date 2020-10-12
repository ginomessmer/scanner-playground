using System.Linq;

namespace Scanner.Shared
{
    public static class ScannerHelper
    {
        public static readonly char[] Numbers = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

        public static bool IsNumber(int c) => Numbers.Contains((char)c);

        public static bool IsCharacter(int c) => 'a' <= c && c <= 'z' || 'A' <= c && c <= 'Z';

        public static bool IsSpacing(int c) => c.Equals(' ') || c.Equals('\t') || c.Equals('\n') || c.Equals('\r');
    }
}