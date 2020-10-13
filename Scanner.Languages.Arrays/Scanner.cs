using Scanner.Shared;
using System;
using System.IO;
using static Scanner.Shared.ScannerHelper;

namespace Scanner.Languages.Arrays
{
    public class Scanner : BaseScanner
    {
        public ArrayState CurrentState { get; private set; } = ArrayState.Whitespace;

        private ArrayTokenType _arrayTokenType = ArrayTokenType.Void;

        public Scanner(StringReader input) : base(input)
        {
        }

        private Token Step(int character, ArrayState newState, ArrayTokenType newArrayTokenType, bool create = false)
        {
            Token res = null;
            if (create)
            {
                res = new Token((int)_arrayTokenType, Text.ToString());
                Text.Clear();
            }

            if (character != IgnoreChar)
                Text.Append((char)character);

            CurrentState = newState;
            _arrayTokenType = newArrayTokenType;

            return res;
        }

        public override Token NextToken()
        {
            Token token = null;
            while (token is null)
            {
                var c = Input.Read();

                token = CurrentState switch
                {
                    ArrayState.Whitespace => c switch
                    {
                        -1 => Step(c, ArrayState.EndOfFile, ArrayTokenType.EndOfFile),

                        ' ' => Step(c, ArrayState.Whitespace, ArrayTokenType.Void),
                        '[' => Step(c, ArrayState.Lsbr, ArrayTokenType.Lsbr),
                        ']' => Step(c, ArrayState.Rsbr, ArrayTokenType.Rsbr),
                        ',' => Step(c, ArrayState.Comma, ArrayTokenType.Comma),

                        var number when IsNumber(number) => Step(c, ArrayState.Number, ArrayTokenType.Number),

                        'n' => Step(c, ArrayState.N, ArrayTokenType.Name),

                        var character when IsCharacter(character) => Step(c, ArrayState.Name, ArrayTokenType.Name),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.Rsbr => c switch
                    {
                        -1 => Step(c, ArrayState.EndOfFile, ArrayTokenType.EndOfFile, true),
                        _ => throw new Exception("Unknown state")
                    },
                    var delimiters when
                        delimiters == ArrayState.Lsbr
                        || delimiters == ArrayState.Rsbr
                        || delimiters == ArrayState.Comma => c switch
                        {
                            var number when IsNumber(number) => Step(c, ArrayState.Number, ArrayTokenType.Number, true),
                            'n' => Step(c, ArrayState.N, ArrayTokenType.Name, true),
                            var character when IsCharacter(character) => Step(c, ArrayState.Name, ArrayTokenType.Name, true),

                            ' ' => Step(c, ArrayState.Whitespace, ArrayTokenType.Void, true),
                            '[' => Step(c, ArrayState.Lsbr, ArrayTokenType.Lsbr, true),
                            ']' => Step(c, ArrayState.Rsbr, ArrayTokenType.Rsbr, true),
                            ',' => Step(c, ArrayState.Comma, ArrayTokenType.Comma, true),

                            _ => throw new ScannerTokenException(c)
                        },
                    ArrayState.Number => c switch
                    {
                        var number when IsNumber(number) => Step(c, ArrayState.Number, ArrayTokenType.Number),
                        var character when IsCharacter(character) => Step(c, ArrayState.Name, ArrayTokenType.Name),

                        ' ' => Step(c, ArrayState.Whitespace, ArrayTokenType.Void, true),
                        '[' => Step(c, ArrayState.Lsbr, ArrayTokenType.Lsbr, true),
                        ']' => Step(c, ArrayState.Rsbr, ArrayTokenType.Rsbr, true),
                        ',' => Step(c, ArrayState.Comma, ArrayTokenType.Comma, true),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.Name => c switch
                    {
                        var input when IsNumber(input) || IsCharacter(input) => Step(c, ArrayState.Name, ArrayTokenType.Name),

                        ' ' => Step(c, ArrayState.Whitespace, ArrayTokenType.Void, true),
                        '[' => Step(c, ArrayState.Lsbr, ArrayTokenType.Lsbr, true),
                        ']' => Step(c, ArrayState.Rsbr, ArrayTokenType.Rsbr, true),
                        ',' => Step(c, ArrayState.Comma, ArrayTokenType.Comma, true),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.N => c switch
                    {
                        'u' => Step(c, ArrayState.Nu, ArrayTokenType.Name),
                        var character when IsCharacter(character) => Step(c, ArrayState.Name, ArrayTokenType.Name),

                        ' ' => Step(c, ArrayState.Whitespace, ArrayTokenType.Void, true),
                        '[' => Step(c, ArrayState.Lsbr, ArrayTokenType.Lsbr, true),
                        ']' => Step(c, ArrayState.Rsbr, ArrayTokenType.Rsbr, true),
                        ',' => Step(c, ArrayState.Comma, ArrayTokenType.Comma, true),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.Nu => c switch
                    {
                        'l' => Step(c, ArrayState.Nul, ArrayTokenType.Name),
                        var character when IsCharacter(character) => Step(c, ArrayState.Name, ArrayTokenType.Name),


                        ' ' => Step(c, ArrayState.Whitespace, ArrayTokenType.Void, true),
                        '[' => Step(c, ArrayState.Lsbr, ArrayTokenType.Lsbr, true),
                        ']' => Step(c, ArrayState.Rsbr, ArrayTokenType.Rsbr, true),
                        ',' => Step(c, ArrayState.Comma, ArrayTokenType.Comma, true),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.Nul => c switch
                    {
                        'l' => Step(c, ArrayState.Null, ArrayTokenType.Null),
                        var character when IsCharacter(character) => Step(c, ArrayState.Name, ArrayTokenType.Name),


                        ' ' => Step(c, ArrayState.Whitespace, ArrayTokenType.Void, true),
                        '[' => Step(c, ArrayState.Lsbr, ArrayTokenType.Lsbr, true),
                        ']' => Step(c, ArrayState.Rsbr, ArrayTokenType.Rsbr, true),
                        ',' => Step(c, ArrayState.Comma, ArrayTokenType.Comma, true),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.Null => c switch
                    {
                        var character when IsCharacter(character) => Step(c, ArrayState.Name, ArrayTokenType.Name),

                        ' ' => Step(c, ArrayState.Whitespace, ArrayTokenType.Void, true),
                        '[' => Step(c, ArrayState.Lsbr, ArrayTokenType.Lsbr, true),
                        ']' => Step(c, ArrayState.Rsbr, ArrayTokenType.Rsbr, true),
                        ',' => Step(c, ArrayState.Comma, ArrayTokenType.Comma, true),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.EndOfFile => Step(IgnoreChar, ArrayState.EndOfFile, ArrayTokenType.EndOfFile, true),
                    _ => throw new ScannerStateException(CurrentState.ToString())
                };
            }

            return token;
        }
    }
}
