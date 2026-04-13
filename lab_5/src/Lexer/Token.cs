namespace Lexer;

public readonly record struct Token
{
    public Token(TokenType type, string lexeme, int position)
    {
        Type = type;
        Lexeme = lexeme;
        Position = position;
    }

    public TokenType Type { get; }

    public string Lexeme { get; }

    public int Position { get; }

    public override string ToString() => $"{Type}: '{Lexeme}' @ {Position}";
}