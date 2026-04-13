using System;

namespace Lexer
{
    #pragma warning disable RCS1194
    public class LexerException : Exception
    {
        public LexerException(string message, int position)
            : base($"{message} (pos={position})")
        {
            Position = position;
        }

        public LexerException()
        {
        }

        public LexerException(string message)
            : base(message)
        {
        }

        public LexerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public int Position { get; }
    }
}