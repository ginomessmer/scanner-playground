namespace NameScanner
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Text { get; set; }

        public Token(TokenType type, string text)
        {
            Type = type;
            Text = text;
        }
    }
}