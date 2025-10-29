namespace Lexer;

public class Token
{
    public TokenType Type { get; }
    public string Value { get; }
    public int Position { get; }
    public int Line { get; }
    public int Column { get; }
    
    public Token(TokenType type, string value, int position, int line = 1, int column = 1)
    {
        Type = type;
        Value = value;
        Position = position;
        Line = line;
        Column = column;
    }
    
    public override string ToString() => $"{Type}('{Value}') at {Position} (L{Line}:C{Column})";
}