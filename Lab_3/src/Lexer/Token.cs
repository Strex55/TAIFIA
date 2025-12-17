using System;

namespace Lexer
{
    public enum TokenType
    {
        Number,
        Identifier,
        Plus,           // +
        Minus,          // -
        Multiply,       // *
        Divide,         // /
        Modulo,         // %
        Power,          // **
        LeftParen,      // (
        RightParen,     // )
        Comma,          // ,
        EndOfFile
    }

    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public int Position { get; }

        public Token(TokenType type, string value, int position)
        {
            Type = type;
            Value = value;
            Position = position;
        }

        public override string ToString()
        {
            return $"Token({Type}, '{Value}', pos={Position})";
        }
    }

    public class SyntaxException : Exception
    {
        public int Position { get; }

        public SyntaxException(string message, int position) : base($"{message} (at position {position})")
        {
            Position = position;
        }
    }
}