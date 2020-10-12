using System;
using System.IO;
using System.Linq;
using System.Text;
using Scanner.Shared;
using static Scanner.Shared.ScannerHelper;

namespace Scanner.Arrays
{
    public class Scanner : IScanner
    {
        private const int IgnoreChar = -1;

        private readonly StringReader _input;
        private readonly StringBuilder _text;

        private ArrayState _state = ArrayState.WS;
        private ArrayTokenType _nameTokenType = ArrayTokenType.Invalid;

        public Scanner(StringReader input)
        {
            _input = input;
            _text = new StringBuilder();
        }

        private Token Step(int c, ArrayState newState, bool create, ArrayTokenType newNameTokenType)
        {
            Token res = null;
            if (create)
            {
                res = new Token((int)_nameTokenType, _text.ToString());
                _text.Clear();
            }

            if (c != IgnoreChar)
                _text.Append((char)c);

            _state = newState;
            _nameTokenType = newNameTokenType;

            return res;
        }

        public Token NextToken()
        {
            Token token = null;
            while (token is null)
            {
                var c = _input.Read();

                token = _state switch
                {
                    ArrayState.WS => c switch
                    {
                        -1 => Step(c, ArrayState.EOF, false, ArrayTokenType.EOF),
                        ' ' => Step(c, ArrayState.WS, false, ArrayTokenType.Invalid),
                        '[' => Step(c, ArrayState.Lsbr, false, ArrayTokenType.Lsbr),
                        ']' => Step(c, ArrayState.Rsbr, false, ArrayTokenType.Rsbr),
                        ',' => Step(c, ArrayState.Comma, false, ArrayTokenType.Comma),

                        var number when IsNumber(number) => Step(c, ArrayState.Number, false, ArrayTokenType.Number),

                        'n' => Step(c, ArrayState.N, false, ArrayTokenType.Id),

                        var character when IsCharacter(character) => Step(c, ArrayState.Name, false, ArrayTokenType.Id),

                        _ => throw new ScannerException(c)
                    },
                    var delimiters when
                        delimiters == ArrayState.Lsbr
                        || delimiters == ArrayState.Rsbr
                        || delimiters == ArrayState.Comma => c switch
                        {
                            -1 => Step(IgnoreChar, ArrayState.EOF, true, ArrayTokenType.EOF),
                            ' ' => Step(IgnoreChar, ArrayState.WS, true, ArrayTokenType.Invalid),
                            '[' => Step(c, ArrayState.Lsbr, true, ArrayTokenType.Lsbr),
                            ']' => Step(c, ArrayState.Rsbr, true, ArrayTokenType.Rsbr),
                            ',' => Step(c, ArrayState.Comma, true, ArrayTokenType.Comma),

                            var number when IsNumber(number) => Step(c, ArrayState.Number, true, ArrayTokenType.Number),

                            'n' => Step(c, ArrayState.N, true, ArrayTokenType.Id),

                            var character when IsCharacter(character) => Step(c, ArrayState.Name, true,
                                ArrayTokenType.Name),

                            _ => throw new ScannerException(c)
                        },
                    ArrayState.Name => c switch
                    {
                        ' ' => Step(IgnoreChar, ArrayState.WS, true, ArrayTokenType.Invalid),
                        var x when IsCharacter(x) => Step(c, ArrayState.Name, false, ArrayTokenType.Invalid),
                        _ => throw new ScannerException(c)
                    }
                };
            }

            return token;
        }
    }

    enum ArrayState
    {
        WS,
        Lsbr,
        Rsbr,
        Comma,
        Number,
        Name,
        N,
        Nu,
        Nul,
        Null,
        EOF
    }

    enum ArrayTokenType
    {
        Invalid = -1,
        Lsbr,
        Rsbr,
        Comma,
        Number,
        Name,
        Id,
        Null,
        EOF
    }
}
