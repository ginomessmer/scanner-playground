using System;
using System.IO;
using Scanner.Shared;
using static Scanner.Shared.ScannerHelper;

namespace Scanner.Languages.ArraysWithFractions
{
    public class Scanner : BaseScanner
    {
        public ArrayState CurrentState { get; private set; } = ArrayState.WS;

        private ArrayTokenType _arrayTokenType = ArrayTokenType.Void;

        public Scanner(StringReader input) : base(input)
        {
        }

        private Token Step(int character, ArrayState newState, ArrayTokenType newNameTokenType, bool create = false)
        {
            Token res = null;
            if (create)
            {
                res = new Token((int)_arrayTokenType, _text.ToString());
                _text.Clear();
            }

            if (character != IgnoreChar)
                _text.Append((char)character);

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
                        -1 => Step(c, ArrayState.EOF, ArrayTokenType.EOF),

                        ' ' => Step(c, ArrayState.WS, ArrayTokenType.Void),
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
                        -1 => Step(c, ArrayState.EOF, ArrayTokenType.EOF, true),
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

                            ' ' => Step(c, ArrayState.WS, ArrayTokenType.Void, true),
                            '[' => Step(c, ArrayState.Lsbr, ArrayTokenType.Lsbr, true),
                            ']' => Step(c, ArrayState.Rsbr, ArrayTokenType.Rsbr, true),
                            ',' => Step(c, ArrayState.Comma, ArrayTokenType.Comma, true),

                            _ => throw new ScannerTokenException(c)
                        },
                    ArrayState.Number => c switch
                    {
                        var number when IsNumber(number) => Step(c, ArrayState.Number, ArrayTokenType.Number),
                        var character when IsCharacter(character) => Step(c, ArrayState.Name, ArrayTokenType.Name),

                        ' ' => Step(c, ArrayState.WS, ArrayTokenType.Void, true),
                        '[' => Step(c, ArrayState.Lsbr, ArrayTokenType.Lsbr, true),
                        ']' => Step(c, ArrayState.Rsbr, ArrayTokenType.Rsbr, true),
                        ',' => Step(c, ArrayState.Comma, ArrayTokenType.Comma, true),

                        '.' => Step(c, ArrayState.Dot, ArrayTokenType.Fraction),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.Name => c switch
                    {
                        var input when IsNumber(input) || IsCharacter(input) => Step(c, ArrayState.Name, ArrayTokenType.Name),

                        ' ' => Step(c, ArrayState.WS, ArrayTokenType.Void, true),
                        '[' => Step(c, ArrayState.Lsbr, ArrayTokenType.Lsbr, true),
                        ']' => Step(c, ArrayState.Rsbr, ArrayTokenType.Rsbr, true),
                        ',' => Step(c, ArrayState.Comma, ArrayTokenType.Comma, true),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.N => c switch
                    {
                        'u' => Step(c, ArrayState.u, ArrayTokenType.Name),
                        var character when IsCharacter(character) => Step(c, ArrayState.Name, ArrayTokenType.Name),

                        ' ' => Step(c, ArrayState.WS, ArrayTokenType.Void, true),
                        '[' => Step(c, ArrayState.Lsbr, ArrayTokenType.Lsbr, true),
                        ']' => Step(c, ArrayState.Rsbr, ArrayTokenType.Rsbr, true),
                        ',' => Step(c, ArrayState.Comma, ArrayTokenType.Comma, true),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.u => c switch
                    {
                        'l' => Step(c, ArrayState.l, ArrayTokenType.Name),
                        var character when IsCharacter(character) => Step(c, ArrayState.Name, ArrayTokenType.Name),


                        ' ' => Step(c, ArrayState.WS, ArrayTokenType.Void, true),
                        '[' => Step(c, ArrayState.Lsbr, ArrayTokenType.Lsbr, true),
                        ']' => Step(c, ArrayState.Rsbr, ArrayTokenType.Rsbr, true),
                        ',' => Step(c, ArrayState.Comma, ArrayTokenType.Comma, true),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.l => c switch
                    {
                        'l' => Step(c, ArrayState.Null, ArrayTokenType.Null),
                        var character when IsCharacter(character) => Step(c, ArrayState.Name, ArrayTokenType.Name),


                        ' ' => Step(c, ArrayState.WS, ArrayTokenType.Void, true),
                        '[' => Step(c, ArrayState.Lsbr, ArrayTokenType.Lsbr, true),
                        ']' => Step(c, ArrayState.Rsbr, ArrayTokenType.Rsbr, true),
                        ',' => Step(c, ArrayState.Comma, ArrayTokenType.Comma, true),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.Null => c switch
                    {
                        var character when IsCharacter(character) => Step(c, ArrayState.Name, ArrayTokenType.Name),

                        ' ' => Step(c, ArrayState.WS, ArrayTokenType.Void, true),
                        '[' => Step(c, ArrayState.Lsbr, ArrayTokenType.Lsbr, true),
                        ']' => Step(c, ArrayState.Rsbr, ArrayTokenType.Rsbr, true),
                        ',' => Step(c, ArrayState.Comma, ArrayTokenType.Comma, true),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.Dot => c switch
                    {
                        var number when IsNumber(number) => Step(c, ArrayState.Fraction, ArrayTokenType.Fraction),
                        '^' => Step(c, ArrayState.Circumflex, ArrayTokenType.Void),

                        ' ' => Step(c, ArrayState.WS, ArrayTokenType.Void, true),
                        '[' => Step(c, ArrayState.Lsbr, ArrayTokenType.Lsbr, true),
                        ']' => Step(c, ArrayState.Rsbr, ArrayTokenType.Rsbr, true),
                        ',' => Step(c, ArrayState.Comma, ArrayTokenType.Comma, true),
                        var character when IsCharacter(character) => Step(c, ArrayState.Name, ArrayTokenType.Name, true),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.Fraction => c switch
                    {
                        var number when IsNumber(number) => Step(c, ArrayState.Fraction, ArrayTokenType.Fraction),
                        '^' => Step(c, ArrayState.Circumflex, ArrayTokenType.Void),

                        ' ' => Step(c, ArrayState.WS, ArrayTokenType.Void, true),
                        '[' => Step(c, ArrayState.Lsbr, ArrayTokenType.Lsbr, true),
                        ']' => Step(c, ArrayState.Rsbr, ArrayTokenType.Rsbr, true),
                        ',' => Step(c, ArrayState.Comma, ArrayTokenType.Comma, true),
                        var character when IsCharacter(character) => Step(c, ArrayState.Name, ArrayTokenType.Name, true),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.Circumflex => c switch
                    {
                        var number when IsNumber(number) => Step(c, ArrayState.Exp, ArrayTokenType.Fraction),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.Exp => c switch
                    {
                        var number when IsNumber(number) => Step(c, ArrayState.Exp, ArrayTokenType.Fraction),

                        ' ' => Step(c, ArrayState.WS, ArrayTokenType.Void, true),
                        '[' => Step(c, ArrayState.Lsbr, ArrayTokenType.Lsbr, true),
                        ']' => Step(c, ArrayState.Rsbr, ArrayTokenType.Rsbr, true),
                        ',' => Step(c, ArrayState.Comma, ArrayTokenType.Comma, true),
                        var character when IsCharacter(character) => Step(c, ArrayState.Name, ArrayTokenType.Name, true),

                        _ => throw new ScannerTokenException(c)
                    },
                    ArrayState.EOF => Step(IgnoreChar, ArrayState.EOF, ArrayTokenType.EOF, true),
                    _ => throw new ScannerStateException(CurrentState.ToString())
                };
            }

            return token;
        }
    }
}
