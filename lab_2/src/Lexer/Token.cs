namespace Astra.Lexer;

public class Token
{
    public TokenType Type { get; }
    public string Value { get; }
    public int Line { get; }
    public int Column { get; }
    public int Position { get; }
    
    public Token(TokenType type, string value, int line, int column, int position)
    {
        Type = type;
        Value = value;
        Line = line;
        Column = column;
        Position = position;
    }
    
    public override string ToString() => $"{Type}('{Value}') at {Line}:{Column}";
}