using System.Text;

namespace Lexer;

public class Lexer
{
    private readonly string _input;
    private int _position;
    private int _line;
    private int _column;

    public Lexer(string input)
    {
        _input = input;
        _position = 0;
        _line = 1;
        _column = 1;
    }

    public IEnumerable<Token> Tokenize()
    {
        while (_position < _input.Length)
        {
            var token = NextToken();
            if (token != null)
                yield return token;
        }
        
        yield return new Token(TokenType.EOF, "", _position, _line, _column);
    }

    private Token? NextToken()
    {
        SkipWhitespace();
        
        if (_position >= _input.Length)
            return null;

        char current = _input[_position];
        
        // Простая логика - пока все символы кроме пробелов это IDENTIFIER
        var token = new Token(TokenType.IDENTIFIER, current.ToString(), _position, _line, _column);
        _position++;
        _column++;
        return token;
    }

    private void SkipWhitespace()
    {
        while (_position < _input.Length && char.IsWhiteSpace(_input[_position]))
        {
            if (_input[_position] == '\n')
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
}