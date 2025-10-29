using System.Collections.Generic;
using System.Text;

namespace Lexer;

public class Lexer
{
    private readonly string _input;
    private int _position;
    private int _line;
    private int _column;

    private readonly Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>
    {
        { "class", TokenType.CLASS },
        { "if", TokenType.IF },
        { "while", TokenType.WHILE },
        { "return", TokenType.RETURN },
        { "void", TokenType.VOID },
        { "int", TokenType.INT },
        { "string", TokenType.STRING },
        { "bool", TokenType.BOOL },
        { "public", TokenType.PUBLIC },
        { "private", TokenType.PRIVATE },
        { "static", TokenType.STATIC }
    };

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
            Token? token = NextToken();
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
        
        // Числовые литералы
        if (char.IsDigit(current))
        {
            return ReadNumber();
        }
        
        // Строковые литералы
        if (current == '"')
        {
            return ReadString();
        }
        
        // Многосимвольные операторы
        if (current == '=')
        {
            if (Peek(1) == '=')
            {
                return CreateToken(TokenType.EQUALS, "==", 2);
            }
            return CreateToken(TokenType.ASSIGN, "=");
        }
        
        if (current == '!')
        {
            if (Peek(1) == '=')
            {
                return CreateToken(TokenType.NOT_EQUALS, "!=", 2);
            }
            return CreateToken(TokenType.NOT, "!");
        }
        
        if (current == '<')
        {
            if (Peek(1) == '=')
            {
                return CreateToken(TokenType.LESS_EQUAL, "<=", 2);
            }
            return CreateToken(TokenType.LESS, "<");
        }
        
        if (current == '>')
        {
            if (Peek(1) == '=')
            {
                return CreateToken(TokenType.GREATER_EQUAL, ">=", 2);
            }
            return CreateToken(TokenType.GREATER, ">");
        }
        
        if (current == '&' && Peek(1) == '&')
        {
            return CreateToken(TokenType.AND, "&&", 2);
        }
        
        if (current == '|' && Peek(1) == '|')
        {
            return CreateToken(TokenType.OR, "||", 2);
        }
        
        // Односимвольные операторы и разделители
        if (current == '+') return CreateToken(TokenType.PLUS, "+");
        if (current == '-') return CreateToken(TokenType.MINUS, "-");
        if (current == '*') return CreateToken(TokenType.MULTIPLY, "*");
        if (current == '/') return CreateToken(TokenType.DIVIDE, "/");
        if (current == '%') return CreateToken(TokenType.MODULO, "%");
        if (current == ';') return CreateToken(TokenType.SEMICOLON, ";");
        if (current == '{') return CreateToken(TokenType.LEFT_BRACE, "{");
        if (current == '}') return CreateToken(TokenType.RIGHT_BRACE, "}");
        if (current == '(') return CreateToken(TokenType.LEFT_PAREN, "(");
        if (current == ')') return CreateToken(TokenType.RIGHT_PAREN, ")");
        if (current == ',') return CreateToken(TokenType.COMMA, ",");
        if (current == '.') return CreateToken(TokenType.DOT, ".");
        
        // Идентификаторы и ключевые слова
        if (char.IsLetter(current) || current == '_')
        {
            return ReadIdentifier();
        }
        
        // Неизвестный символ
        return CreateToken(TokenType.IDENTIFIER, current.ToString());
    }

    private Token ReadNumber()
    {
        int start = _position;
        while (_position < _input.Length && char.IsDigit(_input[_position]))
        {
            _position++;
            _column++;
        }
        
        string number = _input.Substring(start, _position - start);
        return new Token(TokenType.NUMBER, number, start, _line, _column - number.Length);
    }

    private Token ReadString()
    {
        int start = _position;
        _position++; // Пропускаем открывающую кавычку
        _column++;
        
        StringBuilder stringBuilder = new StringBuilder();
        
        while (_position < _input.Length && _input[_position] != '"')
        {
            stringBuilder.Append(_input[_position]);
            _position++;
            _column++;
        }
        
        if (_position < _input.Length && _input[_position] == '"')
        {
            _position++; // Пропускаем закрывающую кавычку
            _column++;
        }
        
        return new Token(TokenType.STRING_LITERAL, stringBuilder.ToString(), start, _line, _column - stringBuilder.Length - 2);
    }

    private Token ReadIdentifier()
    {
        int start = _position;
        while (_position < _input.Length && (char.IsLetterOrDigit(_input[_position]) || _input[_position] == '_'))
        {
            _position++;
            _column++;
        }
        
        string identifier = _input.Substring(start, _position - start);
        
        // Проверяем оператор
        if (_keywords.ContainsKey(identifier))
        {
            return new Token(_keywords[identifier], identifier, start, _line, _column - identifier.Length);
        }
        
        return new Token(TokenType.IDENTIFIER, identifier, start, _line, _column - identifier.Length);
    }

    private Token CreateToken(TokenType type, string value, int length = 1)
    {
        Token token = new Token(type, value, _position, _line, _column);
        _position += length;
        _column += length;
        return token;
    }

    private char Peek(int offset = 1)
    {
        int peekPosition = _position + offset;
        if (peekPosition >= _input.Length)
            return '\0';
        return _input[peekPosition];
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