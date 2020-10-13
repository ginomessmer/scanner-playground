# Scanner Playground
Basic scanner samples for compiler-design written in C#.

Written in Visual Studio 2019. To run tests, execute `dotnet test`. 

## Projects
- [`Scanner.Shared`](Scanner.Shared) contains common types, abstract classes and helpers used across all languages.
- [`Scanner.Tests`](Scanner.Tests) contains token tests for all included languages.

## Languages

### [`Scanner.Languages.Names`](Scanner.Languages.Names)
```ebnf
AS ::= { peter | petra | anna };
```

### [`Scanner.Languages.Arrays` (AS-1)](Scanner.Languages.Arrays)
```ebnf
AS ::= '[' Element { ',' Element } ']';
Element ::= Number | Name | null;
Number ::= '0'..'9' {'0'..'9'};
Name ::= ('0'..'9' | 'a'..'z' | 'A'..'Z') {'0'..'9' | 'a'..'z' | 'A'..'Z'} 
```

### [`Scanner.Languages.ArraysWithFractions` (AS-2)](Scanner.Languages.ArraysWithFractions)
```ebnf
AS ::= '[' Element { ',' Element } ']';
Element ::= Number | Name | Fraction | null;
Number ::= '0'..'9' {'0'..'9'};
Name ::= ('0'..'9' | 'a'..'z' | 'A'..'Z') {'0'..'9' | 'a'..'z' | 'A'..'Z'}
Fraction ::= '0'..'9' {'0'..'9'} '.' {'0'..'9'} [ '^' '0'..'9' {'0'..'9'} ];
```
