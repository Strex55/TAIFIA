namespace Lexer;

public enum TokenType
{
    // Конец файла
    EOF,
    
    // Ключевые слова C#
    CLASS, IF, WHILE, RETURN, VOID, 
    INT, STRING, BOOL, PUBLIC, PRIVATE, STATIC,
    
    // Идентификаторы и литералы
    IDENTIFIER, NUMBER, STRING_LITERAL,
    
    // Операторы
    ASSIGN, PLUS, MINUS, MULTIPLY, DIVIDE, MODULO,
    EQUALS, NOT_EQUALS, LESS, GREATER, LESS_EQUAL, GREATER_EQUAL,
    AND, OR, NOT,
    INCREMENT, DECREMENT,
    
    // Разделители
    LEFT_BRACE, RIGHT_BRACE, 
    LEFT_PAREN, RIGHT_PAREN,
    SEMICOLON, COMMA, DOT
}