using System;

namespace Scanner.Shared
{
    public class ScannerException : Exception
    {
        private readonly int _character;
        public override string Message => $"Unexpected character0 '{(char)_character}'";

        public ScannerException(int character)
        {
            _character = character;
        }
    }
}