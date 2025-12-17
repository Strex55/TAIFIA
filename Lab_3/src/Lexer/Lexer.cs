using System;
using System.Text;

namespace Lexer
{
    /*
    Лексический анализатор для языка Astra.
    Разбивает входную строку на токены согласно грамматике.
    */
    public class Lexer
    {
        private readonly string _input;
        private int _position;
        private int _length;

        public Lexer(string input)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _position = 0;
            _length = input.Length;
        }

        /*
        Разбирает следующий токен из входной строки.
        Возвращает токен или Token.EndOfFile в конце строки.
        */
        public Token ParseToken()
        {
            SkipWhitespace();

            if (_position >= _length)
            {
                return new Token(TokenType.EndOfFile, "", _position);
            }

            char current = _input[_position];

            // Числа
            if (char.IsDigit(current) || current == '.')
            {
                return ParseNumber();
            }

            // Идентификаторы и константы
            if (char.IsLetter(current) || current == '_')
            {
                return ParseIdentifier();
            }

            // Операторы и символы
            switch (current)
            {
                case '+':
                    _position++;
                    return new Token(TokenType.Plus, "+", _position - 1);

                case '-':
                    _position++;
                    return new Token(TokenType.Minus, "-", _position - 1);

                case '*':
                    _position++;
                    if (_position < _length && _input[_position] == '*')
                    {
                        _position++;
                        return new Token(TokenType.Power, "**", _position - 2);
                    }
                    return new Token(TokenType.Multiply, "*", _position - 1);

                case '/':
                    _position++;
                    return new Token(TokenType.Divide, "/", _position - 1);

                case '%':
                    _position++;
                    return new Token(TokenType.Modulo, "%", _position - 1);

                case '(':
                    _position++;
                    return new Token(TokenType.LeftParen, "(", _position - 1);

                case ')':
                    _position++;
                    return new Token(TokenType.RightParen, ")", _position - 1);

                case ',':
                    _position++;
                    return new Token(TokenType.Comma, ",", _position - 1);

                default:
                    throw new SyntaxException($"Unexpected character: '{current}'", _position);
            }
        }

        private Token ParseNumber()
        {
            int start = _position;
            bool hasDot = false;

            while (_position < _length)
            {
                char c = _input[_position];
                if (char.IsDigit(c))
                {
                    _position++;
                }
                else if (c == '.' && !hasDot)
                {
                    hasDot = true;
                    _position++;
                }
                else
                {
                    break;
                }
            }

            string value = _input.Substring(start, _position - start);
            return new Token(TokenType.Number, value, start);
        }

        private Token ParseIdentifier()
        {
            int start = _position;

            while (_position < _length)
            {
                char c = _input[_position];
                if (char.IsLetterOrDigit(c) || c == '_')
                {
                    _position++;
                }
                else
                {
                    break;
                }
            }

            string value = _input.Substring(start, _position - start);
            return new Token(TokenType.Identifier, value, start);
        }

        private void SkipWhitespace()
        {
            while (_position < _length && char.IsWhiteSpace(_input[_position]))
            {
                _position++;
            }
        }
    }
}