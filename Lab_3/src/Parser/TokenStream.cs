using System;
using Lexer;

namespace Parser
{
    public class TokenStream
    {
        private readonly Lexer.Lexer _lexer;
        private Token _currentToken;
        private Token _nextToken;

        public TokenStream(string code)
        {
            _lexer = new Lexer.Lexer(code);
            _currentToken = _lexer.ParseToken();
            _nextToken = _lexer.ParseToken();
        }

        public Token Peek()
        {
            return _currentToken;
        }

        public Token PeekNext()
        {
            return _nextToken;
        }

        public void Advance()
        {
            _currentToken = _nextToken;
            _nextToken = _lexer.ParseToken();
        }

        public bool Match(TokenType expectedType)
        {
            return _currentToken.Type == expectedType;
        }

        public bool Match(TokenType expectedType, string expectedValue)
        {
            return _currentToken.Type == expectedType && 
                   _currentToken.Value == expectedValue;
        }

        public void Consume(TokenType expectedType, string errorMessage)
        {
            if (!Match(expectedType))
            {
                throw new ParserException(errorMessage, _currentToken.Position);
            }
            Advance();
        }

        public bool TryConsume(TokenType expectedType)
        {
            if (Match(expectedType))
            {
                Advance();
                return true;
            }
            return false;
        }

        public bool TryConsume(TokenType expectedType, string expectedValue)
        {
            if (Match(expectedType, expectedValue))
            {
                Advance();
                return true;
            }
            return false;
        }
    }

}