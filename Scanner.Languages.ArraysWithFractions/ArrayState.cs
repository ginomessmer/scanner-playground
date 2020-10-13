namespace Scanner.Languages.ArraysWithFractions
{
    public enum ArrayState
    {
        EOF = -1,
        WS,
        Lsbr,
        Rsbr,
        Comma,
        Number,
        Name,
        N,
        u,
        l,
        Null,
        Dot,
        Fraction,
        Circumflex,
        Exp
    }
}