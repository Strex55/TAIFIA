namespace Astra.Lexer;

public enum TokenType
{
    // Ключевые слова
    Start, End, Namespace, Import, Let, Const, Func, Return,
    If, Else, For, In, While, Break, Continue, Show, True, False, Null, Type,
    
    // Идентификаторы
    Identifier,
    
    // Литералы
    NumberLiteral, StringLiteral, BooleanLiteral, NullLiteral,
    
    // Операторы
    Plus, Minus, Multiply, Divide, Modulo, Power, // + - * / % **
    Equal, NotEqual, Less, LessEqual, Greater, GreaterEqual, // == != < <= > >=
    And, Or, Not, // and or not
    Assign, PlusAssign, MinusAssign, MultiplyAssign, DivideAssign, ModuloAssign, // = += -= *= /= %=
    ReturnType, // ->
    
    // Разделители
    Comma, Colon, Semicolon, 
    LeftParen, RightParen,    // ( )
    LeftBracket, RightBracket, // [ ]
    LeftBrace, RightBrace,    // { }
    
    // Специальные
    EndOfFile,
    Error
}