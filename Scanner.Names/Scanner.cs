using Scanner.Shared;
using System;
using System.IO;
using System.Text;

namespace Scanner.Names
{
    public class Scanner
    {
        private const int IgnoreChar = -1;

        private readonly StringReader _input;
        private readonly StringBuilder _text;

        private NameState _state = NameState.WS;
        private TokenType _tokenType = TokenType.Invalid;

        public Scanner(StringReader input)
        {
            _input = input;
            _text = new StringBuilder();
        }

        private Token Step(int c, NameState newState, bool create, TokenType newTokenType)
        {
            Token res = null;
            if (create)
            {
                res = new Token((int)_tokenType, _text.ToString());
                _text.Clear();
            }

            if (c != IgnoreChar)
                _text.Append((char)c);

            _state = newState;
            _tokenType = newTokenType;

            return res;
        }

        public Token NextToken()
        {
            Token token = null;
            while (token == null)
            {
                var c = _input.Read();

                token = _state switch
                {
                    NameState.WS => c switch
                    {
                        -1 => Step(IgnoreChar, NameState.EOF, false, TokenType.EOF),
                        ' ' => Step(IgnoreChar, NameState.WS, false, TokenType.Invalid),
                        'a' => Step(c, NameState.A, false, TokenType.Invalid),
                        'p' => Step(c, NameState.P, false, TokenType.Invalid),
                        _ => throw new ScannerException(c)
                    },
                    NameState.P => c switch
                    {
                        'e' => Step(c, NameState.Pe, false, TokenType.Invalid),
                        _ => throw new ScannerException(c)
                    },
                    NameState.Pe => c switch
                    {
                        't' => Step(c, NameState.Pet, false, TokenType.Invalid),
                        _ => throw new ScannerException(c)
                    },
                    NameState.Pet => c switch
                    {
                        'e' => Step(c, NameState.Pete, false, TokenType.Invalid),
                        'r' => Step(c, NameState.Petr, false, TokenType.Invalid),
                        _ => throw new ScannerException(c)
                    },
                    NameState.Pete => c switch
                    {
                        'r' => Step(c, NameState.Peter, false, TokenType.Peter),
                        _ => throw new ScannerException(c)
                    },
                    NameState.Petr => c switch
                    {
                        'a' => Step(c, NameState.Petra, false, TokenType.Petra),
                        _ => throw new ScannerException(c)
                    },
                    NameState.A => c switch
                    {
                        'n' => Step(c, NameState.An, false, TokenType.Invalid),
                        _ => throw new ScannerException(c)
                    },
                    NameState.An => c switch
                    {
                        'n' => Step(c, NameState.Ann, false, TokenType.Invalid),
                        _ => throw new ScannerException(c),
                    },
                    NameState.Ann => c switch
                    {
                        'a' => Step(c, NameState.Anna, false, TokenType.Anna),
                        _ => throw new ScannerException(c),
                    },
                    var x
                        when x == NameState.Peter || x == NameState.Petra || x == NameState.Anna => c switch
                        {
                            -1 => Step(IgnoreChar, NameState.EOF, true, TokenType.EOF),
                            var z when z.Equals(' ')
                                       || z.Equals('\t')
                                       || z.Equals('\n')
                                       || z.Equals('\r') => Step(IgnoreChar, NameState.WS, true, TokenType.Invalid),
                            'a' => Step(c, NameState.A, true, TokenType.Invalid),
                            'p' => Step(c, NameState.P, true, TokenType.Invalid),
                            _ => throw new ScannerException(c)
                        },
                    NameState.EOF => Step(IgnoreChar, NameState.EOF, true, TokenType.EOF),
                    _ => throw new Exception($"Unexpected state {_state}")
                };
            }

            return token;
        }
    }
}