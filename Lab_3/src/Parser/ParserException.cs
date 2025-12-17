using System;

namespace Parser
{
    public class ParserException : Exception
    {
        public int Position { get; }

        public ParserException(string message, int position) : base($"{message} (at position {position})")
        {
            Position = position;
        }
    }
}