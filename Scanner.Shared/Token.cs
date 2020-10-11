namespace Scanner.Shared
{
    public class Token
    {
        public int Type { get; set; }
        public string Text { get; set; }

        public Token(int type, string text)
        {
            Type = type;
            Text = text;
        }
    }
}