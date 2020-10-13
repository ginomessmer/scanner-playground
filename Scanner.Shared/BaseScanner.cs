using System.IO;
using System.Text;

namespace Scanner.Shared
{
    public abstract class BaseScanner : IScanner
    {
        protected readonly StringReader Input;
        protected readonly StringBuilder Text;

        protected BaseScanner(StringReader input)
        {
            Input = input;
            Text = new StringBuilder();
        }

        public abstract Token NextToken();
    }
}