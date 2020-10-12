using System.IO;
using System.Text;

namespace Scanner.Shared
{
    public interface IScanner
    {
        Token NextToken();
    }

    public abstract class BaseScanner : IScanner
    {
        protected readonly StringReader _input;
        protected readonly StringBuilder _text;

        protected BaseScanner(StringReader input)
        {
            _input = input;
            _text = new StringBuilder();
        }

        public abstract Token NextToken();
    }
}