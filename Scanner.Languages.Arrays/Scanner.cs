using Scanner.Shared;
using System;
using System.IO;
using static Scanner.Shared.ScannerHelper;

namespace Scanner.Languages.Arrays
{
    public class Scanner : BaseScanner
    {
        public ArrayState CurrentState { get; private set; } = ArrayState.WS;

        private ArrayTokenType _arrayTokenType = ArrayTokenType.Invalid;

        public Scanner(StringReader input) : base(input)
        {
        }

        private Token Step(int c, ArrayState newState, bool create, ArrayTokenType newNameTokenType)
        {
            Token res = null;
            if (create)
            {
                res = new Token((int)_arrayTokenType, _text.ToString());
                _text.Clear();
            }

            if (c != IgnoreChar)
                _text.Append((char)c);

            CurrentState = newState;
            _arrayTokenType = newNameTokenType;

            return res;
        }

        public override Token NextToken()
        {
            Token token = null;
            while (token is null)
            {
                var c = _input.Read();

                token = CurrentState switch
                {
                    ArrayState.WS => c switch
                    {
                        -1 => Step(c, ArrayState.EOF, false, ArrayTokenType.EOF),

                        ' ' => Step(c, ArrayState.WS, false, ArrayTokenType.Invalid),
                        '[' => Step(c, ArrayState.Lsbr, false, ArrayTokenType.Lsbr),
                        ']' => Step(c, ArrayState.Rsbr, false, ArrayTokenType.Rsbr),
                        ',' => Step(c, ArrayState.Comma, false, ArrayTokenType.Comma),

                        var number when IsNumber(number) => Step(c, ArrayState.Number, false, ArrayTokenType.Number),

                        'n' => Step(c, ArrayState.N, false, ArrayTokenType.Name),

                        var character when IsCharacter(character) => Step(c, ArrayState.Name, false, ArrayTokenType.Name),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.Rsbr => c switch
                    {
                        -1 => Step(c, ArrayState.EOF, true, ArrayTokenType.EOF),
                        _ => throw new Exception("Unknown state")
                    },
                    var delimiters when
                        delimiters == ArrayState.Lsbr
                        || delimiters == ArrayState.Rsbr
                        || delimiters == ArrayState.Comma => c switch
                        {
                            var number when IsNumber(number) => Step(c, ArrayState.Number, true, ArrayTokenType.Number),
                            'n' => Step(c, ArrayState.N, true, ArrayTokenType.Name),
                            var character when IsCharacter(character) => Step(c, ArrayState.Name, true, ArrayTokenType.Name),

                            ' ' => Step(c, ArrayState.WS, true, ArrayTokenType.Invalid),
                            '[' => Step(c, ArrayState.Lsbr, true, ArrayTokenType.Lsbr),
                            ']' => Step(c, ArrayState.Rsbr, true, ArrayTokenType.Rsbr),
                            ',' => Step(c, ArrayState.Comma, true, ArrayTokenType.Comma),

                            _ => throw new ScannerTokenException(c)
                        },
                    ArrayState.Number => c switch
                    {
                        var number when IsNumber(number) => Step(c, ArrayState.Number, false, ArrayTokenType.Number),
                        var character when IsCharacter(character) => Step(c, ArrayState.Name, false, ArrayTokenType.Name),

                        ' ' => Step(c, ArrayState.WS, true, ArrayTokenType.Invalid),
                        '[' => Step(c, ArrayState.Lsbr, true, ArrayTokenType.Lsbr),
                        ']' => Step(c, ArrayState.Rsbr, true, ArrayTokenType.Rsbr),
                        ',' => Step(c, ArrayState.Comma, true, ArrayTokenType.Comma),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.Name => c switch
                    {
                        var input when IsNumber(input) || IsCharacter(input) => Step(c, ArrayState.Name, false, ArrayTokenType.Name),

                        ' ' => Step(c, ArrayState.WS, true, ArrayTokenType.Invalid),
                        '[' => Step(c, ArrayState.Lsbr, true, ArrayTokenType.Lsbr),
                        ']' => Step(c, ArrayState.Rsbr, true, ArrayTokenType.Rsbr),
                        ',' => Step(c, ArrayState.Comma, true, ArrayTokenType.Comma),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.N => c switch
                    {
                        'u' => Step(c, ArrayState.u, false, ArrayTokenType.Name),
                        var character when IsCharacter(character) => Step(c, ArrayState.Name, false, ArrayTokenType.Name),

                        ' ' => Step(c, ArrayState.WS, true, ArrayTokenType.Invalid),
                        '[' => Step(c, ArrayState.Lsbr, true, ArrayTokenType.Lsbr),
                        ']' => Step(c, ArrayState.Rsbr, true, ArrayTokenType.Rsbr),
                        ',' => Step(c, ArrayState.Comma, true, ArrayTokenType.Comma),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.u => c switch
                    {
                        'l' => Step(c, ArrayState.l, false, ArrayTokenType.Name),
                        var character when IsCharacter(character) => Step(c, ArrayState.Name, false, ArrayTokenType.Name),


                        ' ' => Step(c, ArrayState.WS, true, ArrayTokenType.Invalid),
                        '[' => Step(c, ArrayState.Lsbr, true, ArrayTokenType.Lsbr),
                        ']' => Step(c, ArrayState.Rsbr, true, ArrayTokenType.Rsbr),
                        ',' => Step(c, ArrayState.Comma, true, ArrayTokenType.Comma),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.l => c switch
                    {
                        'l' => Step(c, ArrayState.Null, false, ArrayTokenType.Null),
                        var character when IsCharacter(character) => Step(c, ArrayState.Name, false, ArrayTokenType.Name),


                        ' ' => Step(c, ArrayState.WS, true, ArrayTokenType.Invalid),
                        '[' => Step(c, ArrayState.Lsbr, true, ArrayTokenType.Lsbr),
                        ']' => Step(c, ArrayState.Rsbr, true, ArrayTokenType.Rsbr),
                        ',' => Step(c, ArrayState.Comma, true, ArrayTokenType.Comma),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.Null => c switch
                    {
                        var character when IsCharacter(character) => Step(c, ArrayState.Name, false, ArrayTokenType.Name),

                        ' ' => Step(c, ArrayState.WS, true, ArrayTokenType.Invalid),
                        '[' => Step(c, ArrayState.Lsbr, true, ArrayTokenType.Lsbr),
                        ']' => Step(c, ArrayState.Rsbr, true, ArrayTokenType.Rsbr),
                        ',' => Step(c, ArrayState.Comma, true, ArrayTokenType.Comma),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.EOF => Step(IgnoreChar, ArrayState.EOF, true, ArrayTokenType.EOF),
                    _ => throw new ScannerStateException(CurrentState.ToString())
                };
            }

            return token;
        }
    }
}
