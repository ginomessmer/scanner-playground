using System;

namespace Scanner.Shared
{
    public class ScannerStateException : Exception
    {
        public string State { get; }
        public override string Message => $"Invalid state {State}";

        public ScannerStateException(string state)
        {
            State = state;
        }
    }
}