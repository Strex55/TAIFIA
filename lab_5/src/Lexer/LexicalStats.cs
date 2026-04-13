using System.Text;

namespace Lexer;

public struct LexicalData
{
    public int Keywords { get; set; }

    public int Identifiers { get; set; }

    public int NumberLiterals { get; set; }

    public int StringLiterals { get; set; }

    public int Operators { get; set; }

    public int OtherLexemes { get; set; }

    public override readonly string ToString()
    {
        return $"""
            keywords: {Keywords}
            identifier: {Identifiers}
            number literals: {NumberLiterals}
            string literals: {StringLiterals}
            operators: {Operators}
            other lexemes: {OtherLexemes}
        """;
    }
}

public static class LexicalStats
{
    public static string CollectFromFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Файл не найден", path);
        }

        string text = File.ReadAllText(path, Encoding.UTF8);
        Lexer lexer = new Lexer(text);
        IReadOnlyList<Token> tokens = lexer.Tokenize();

        LexicalData stats = default;
        HashSet<string> seenIdentifiers = new HashSet<string>(StringComparer.Ordinal);

        foreach (Token token in tokens)
        {
            switch (token.Type)
            {
                case TokenType.Bello:
                    // bello! не считается ключевым словом для статистики
                    break;
                case TokenType.Loka:
                    stats.OtherLexemes++;
                    break;
                case TokenType.Oca:
                case TokenType.Stopa:
                case TokenType.Bapple:
                case TokenType.Poop:
                case TokenType.Trusela:
                case TokenType.BiDo:
                case TokenType.UhOh:
                case TokenType.Again:
                case TokenType.Kemari:
                case TokenType.Aspetta:
                case TokenType.Tulalilloo:
                case TokenType.Ti:
                case TokenType.Amo:
                case TokenType.Guoleila:
                case TokenType.Tank:
                case TokenType.Yu:
                case TokenType.BooYa:
                case TokenType.Naidu:
                case TokenType.Da:
                case TokenType.No:
                    stats.Keywords++;
                    break;
                case TokenType.Identifier:
                    if (IsTypeName(token.Lexeme))
                    {
                        stats.OtherLexemes++;
                    }
                    else if (seenIdentifiers.Add(token.Lexeme))
                    {
                        stats.Identifiers++;
                    }

                    break;
                case TokenType.NumberLiteral:
                    stats.NumberLiterals++;
                    break;
                case TokenType.StringLiteral:
                    stats.StringLiterals++;
                    break;
                case TokenType.Operator:
                    stats.Operators++;
                    break;
                case TokenType.Error:
                    stats.OtherLexemes++;
                    break;
                case TokenType.Whitespace:
                case TokenType.Comment:
                    break;
                default:
                    stats.OtherLexemes++;
                    break;
            }
        }

        return stats.ToString();
    }

    private static bool IsTypeName(string lexeme)
    {
        return string.Equals(lexeme, "Banana", StringComparison.Ordinal)
            || string.Equals(lexeme, "Papaya", StringComparison.Ordinal)
            || string.Equals(lexeme, "Gelato", StringComparison.Ordinal)
            || string.Equals(lexeme, "Spaghetti", StringComparison.Ordinal);
    }
}