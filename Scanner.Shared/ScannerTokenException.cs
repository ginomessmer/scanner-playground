using System;

namespace Scanner.Shared
{
    public class ScannerTokenException : Exception
    {
        public char Character { get; }
        public override string Message => $"Unexpected character '{Character}'";

        public ScannerTokenException(int character)
        {
            Character = (char) character;
        }
    }
}