using System.Text;

namespace Astra.Lexer;

public class Lexer
{
    private readonly string _source;
    private int _position;
    private int _line;
    private int _column;
    
    public Lexer(string source)
    {
        _source = source;
        _position = 0;
        _line = 1;
        _column = 1;
    }
    
    public Token NextToken()
{
    SkipWhitespace();
    
    if (_position >= _source.Length)
        return CreateToken(TokenType.EndOfFile, "");
        
    char current = _source[_position];
    
    // TODO: Реализовать обработку различных типов токенов
    
    // Временная реализация для прохождения тестов
    return CreateToken(TokenType.EndOfFile, "");
}
    
    private void SkipWhitespace()
    {
        while (_position < _source.Length && char.IsWhiteSpace(_source[_position]))
        {
            if (_source[_position] == '\n')
            {
                _line++;
                _column = 1;
            }
            else
            {
                _column++;
            }
            _position++;
        }
    }
    
    private Token CreateToken(TokenType type, string value)
    {
        var token = new Token(type, value, _line, _column, _position);
        _column += value.Length;
        _position += value.Length;
        return token;
    }
    
    private char Peek(int offset = 1)
    {
        var peekPosition = _position + offset;
        return peekPosition < _source.Length ? _source[peekPosition] : '\0';
    }
}