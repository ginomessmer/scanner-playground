using Scanner.Shared;
using System;
using System.IO;
using System.Text;

namespace Scanner.Names
{
    public class Scanner : IScanner
    {
        private const int IgnoreChar = -1;

        private readonly StringReader _input;
        private readonly StringBuilder _text;

        private NameState _state = NameState.WS;
        private NameTokenType _nameTokenType = NameTokenType.Invalid;

        public Scanner(StringReader input)
        {
            _input = input;
            _text = new StringBuilder();
        }

        private Token Step(int c, NameState newState, bool create, NameTokenType newNameTokenType)
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
                    NameState.WS => c switch
                    {
                        -1 => Step(IgnoreChar, NameState.EOF, false, NameTokenType.EOF),
                        ' ' => Step(IgnoreChar, NameState.WS, false, NameTokenType.Invalid),
                        'a' => Step(c, NameState.A, false, NameTokenType.Invalid),
                        'p' => Step(c, NameState.P, false, NameTokenType.Invalid),
                        _ => throw new ScannerException(c)
                    },
                    NameState.P => c switch
                    {
                        'e' => Step(c, NameState.Pe, false, NameTokenType.Invalid),
                        _ => throw new ScannerException(c)
                    },
                    NameState.Pe => c switch
                    {
                        't' => Step(c, NameState.Pet, false, NameTokenType.Invalid),
                        _ => throw new ScannerException(c)
                    },
                    NameState.Pet => c switch
                    {
                        'e' => Step(c, NameState.Pete, false, NameTokenType.Invalid),
                        'r' => Step(c, NameState.Petr, false, NameTokenType.Invalid),
                        _ => throw new ScannerException(c)
                    },
                    NameState.Pete => c switch
                    {
                        'r' => Step(c, NameState.Peter, false, NameTokenType.Peter),
                        _ => throw new ScannerException(c)
                    },
                    NameState.Petr => c switch
                    {
                        'a' => Step(c, NameState.Petra, false, NameTokenType.Petra),
                        _ => throw new ScannerException(c)
                    },
                    NameState.A => c switch
                    {
                        'n' => Step(c, NameState.An, false, NameTokenType.Invalid),
                        _ => throw new ScannerException(c)
                    },
                    NameState.An => c switch
                    {
                        'n' => Step(c, NameState.Ann, false, NameTokenType.Invalid),
                        _ => throw new ScannerException(c),
                    },
                    NameState.Ann => c switch
                    {
                        'a' => Step(c, NameState.Anna, false, NameTokenType.Anna),
                        _ => throw new ScannerException(c),
                    },
                    var x
                        when x == NameState.Peter || x == NameState.Petra || x == NameState.Anna => c switch
                        {
                            -1 => Step(IgnoreChar, NameState.EOF, true, NameTokenType.EOF),
                            var z when z.Equals(' ')
                                       || z.Equals('\t')
                                       || z.Equals('\n')
                                       || z.Equals('\r') => Step(IgnoreChar, NameState.WS, true, NameTokenType.Invalid),
                            'a' => Step(c, NameState.A, true, NameTokenType.Invalid),
                            'p' => Step(c, NameState.P, true, NameTokenType.Invalid),
                            _ => throw new ScannerException(c)
                        },
                    NameState.EOF => Step(IgnoreChar, NameState.EOF, true, NameTokenType.EOF),
                    _ => throw new Exception($"Unexpected state {_state}")
                };
            }

            return token;
        }
    }
}