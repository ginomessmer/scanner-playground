using Scanner.Shared;
using System.IO;
using static Scanner.Shared.ScannerHelper;

namespace Scanner.Languages.Names
{
    public class Scanner : BaseScanner
    {
        public NameState CurrentState { get; private set; } = NameState.Whitespace;

        private NameTokenType _nameTokenType = NameTokenType.Void;

        public Scanner(StringReader input) : base(input)
        {
        }

        private Token Step(int character, NameState newState, NameTokenType newNameTokenType, bool create = false)
        {
            Token res = null;
            if (create)
            {
                res = new Token((int)_nameTokenType, _text.ToString());
                _text.Clear();
            }

            if (character != IgnoreChar)
                _text.Append((char)character);

            CurrentState = newState;
            _nameTokenType = newNameTokenType;

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
                    NameState.Whitespace => c switch
                    {
                        -1 => Step(IgnoreChar, NameState.EndOfFile, NameTokenType.EndOfFile),
                        ' ' => Step(IgnoreChar, NameState.Whitespace, NameTokenType.Void),
                        'a' => Step(c, NameState.A, NameTokenType.Void),
                        'p' => Step(c, NameState.P, NameTokenType.Void),
                        _ => throw new ScannerTokenException(c)
                    },
                    NameState.P => c switch
                    {
                        'e' => Step(c, NameState.Pe, NameTokenType.Void),
                        _ => throw new ScannerTokenException(c)
                    },
                    NameState.Pe => c switch
                    {
                        't' => Step(c, NameState.Pet, NameTokenType.Void),
                        _ => throw new ScannerTokenException(c)
                    },
                    NameState.Pet => c switch
                    {
                        'e' => Step(c, NameState.Pete, NameTokenType.Void),
                        'r' => Step(c, NameState.Petr, NameTokenType.Void),
                        _ => throw new ScannerTokenException(c)
                    },
                    NameState.Pete => c switch
                    {
                        'r' => Step(c, NameState.Peter, NameTokenType.Peter),
                        _ => throw new ScannerTokenException(c)
                    },
                    NameState.Petr => c switch
                    {
                        'a' => Step(c, NameState.Petra, NameTokenType.Petra),
                        _ => throw new ScannerTokenException(c)
                    },
                    NameState.A => c switch
                    {
                        'n' => Step(c, NameState.An, NameTokenType.Void),
                        _ => throw new ScannerTokenException(c)
                    },
                    NameState.An => c switch
                    {
                        'n' => Step(c, NameState.Ann, NameTokenType.Void),
                        _ => throw new ScannerTokenException(c),
                    },
                    NameState.Ann => c switch
                    {
                        'a' => Step(c, NameState.Anna, NameTokenType.Anna),
                        _ => throw new ScannerTokenException(c),
                    },
                    var x
                        when x == NameState.Peter || x == NameState.Petra || x == NameState.Anna => c switch
                        {
                            -1 => Step(IgnoreChar, NameState.EndOfFile, NameTokenType.EndOfFile, true),
                            var z when IsSpacing(z) => Step(IgnoreChar, NameState.Whitespace, NameTokenType.Void, true),
                            'a' => Step(c, NameState.A, NameTokenType.Void, true),
                            'p' => Step(c, NameState.P, NameTokenType.Void, true),
                            _ => throw new ScannerTokenException(c)
                        },
                    NameState.EndOfFile => Step(IgnoreChar, NameState.EndOfFile, NameTokenType.EndOfFile, true),
                    _ => throw new ScannerStateException(CurrentState.ToString())
                };
            }

            return token;
        }
    }
}