using System.Collections.Generic;

namespace Scanner.Shared
{
    public class Token
    {
        public int Type { get; }
        public string Text { get; }

        public Token(int type, string text)
        {
            Type = type;
            Text = text;
        }

        public Token SetText(string text) => new Token(Type, text);
    }
}