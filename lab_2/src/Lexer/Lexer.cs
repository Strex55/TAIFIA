using System.Text;

namespace Astra.Lexer;

public class Lexer
{
    private readonly string source;
    private readonly Position position;

    private static readonly Dictionary<string, TokenType> Keywords =
     new Dictionary<string, TokenType>()
     {
         ["start"] = TokenType.Start,
         ["end"] = TokenType.End,
         ["namespace"] = TokenType.Namespace,
         ["import"] = TokenType.Import,
         ["let"] = TokenType.Let,
         ["const"] = TokenType.Const,
         ["func"] = TokenType.Func,
         ["return"] = TokenType.Return,
         ["if"] = TokenType.If,
         ["else"] = TokenType.Else,
         ["for"] = TokenType.For,
         ["in"] = TokenType.In,
         ["while"] = TokenType.While,
         ["break"] = TokenType.Break,
         ["continue"] = TokenType.Continue,
         ["show"] = TokenType.Show,
         ["true"] = TokenType.True,
         ["false"] = TokenType.False,
         ["null"] = TokenType.Null,
         ["type"] = TokenType.Type,
         ["and"] = TokenType.And,
         ["or"] = TokenType.Or,  
         ["not"] = TokenType.Not,
     };

    public Lexer(string source)
    {
        this.source = source;
        this.position = new Position();
    }

    public Token NextToken()
    {
        SkipWhitespace();

        if (position.Absolute >= source.Length)
        {
            return CreateToken(TokenType.EndOfFile, "");
        }

        char current = CurrentChar();

        if (current == '#')
        {
            SkipSingleLineComment();
            return NextToken();
        }

        if (current == '/' && Peek() == '*')
        {
            SkipMultiLineComment();
            return NextToken();
        }

        if (current == '"' && Peek() == '"' && Peek(2) == '"')
        {
            return ReadMultiLineString();
        }

        if (current == '"' || current == '\'')
        {
            return ReadString();
        }

        if (char.IsLetter(current) || current == '_')
        {
            return ReadIdentifierOrKeyword();
        }

        if (char.IsDigit(current) || (current == '-' && char.IsDigit(Peek())))
        {
            return ReadNumber();
        }

        switch (current)
        {
            case '+': return ReadPlusOperator();
            case '-': return ReadMinusOperator();
            case '*': return ReadMultiplyOperator();
            case '/': return ReadDivideOperator();
            case '%': return ReadModuloOperator();
            case '=': return ReadEqualOperator();
            case '!': return ReadNotEqualOperator();
            case '<': return ReadLessOperator();
            case '>': return ReadGreaterOperator();
            case ',': Advance(); return CreateToken(TokenType.Comma, ",");
            case ':': Advance(); return CreateToken(TokenType.Colon, ":");
            case ';': Advance(); return CreateToken(TokenType.Semicolon, ";");
            case '(': Advance(); return CreateToken(TokenType.LeftParen, "(");
            case ')': Advance(); return CreateToken(TokenType.RightParen, ")");
            case '[': Advance(); return CreateToken(TokenType.LeftBracket, "[");
            case ']': Advance(); return CreateToken(TokenType.RightBracket, "]");
            case '{': Advance(); return CreateToken(TokenType.LeftBrace, "{");
            case '}': Advance(); return CreateToken(TokenType.RightBrace, "}");
        }

        Token errorToken = CreateToken(TokenType.Error, current.ToString());
        Advance();
        return errorToken;
    }

    private Token ReadIdentifierOrKeyword()
    {
        Position start = position.Clone();
        StringBuilder value = new StringBuilder();

        while (position.Absolute < source.Length &&
               (char.IsLetterOrDigit(CurrentChar()) || CurrentChar() == '_'))
        {
            value.Append(CurrentChar());
            Advance();
        }

        string identifier = value.ToString();

        if (Keywords.TryGetValue(identifier, out TokenType keywordType))
        {
            return CreateToken(keywordType, identifier, start);
        }

        return CreateToken(TokenType.Identifier, identifier, start);
    }

    private Token ReadNumber()
    {
        Position start = position.Clone();
        StringBuilder value = new StringBuilder();

        if (CurrentChar() == '-')
        {
            value.Append(CurrentChar());
            Advance();
        }

        if (CurrentChar() == '0' && (Peek() == 'x' || Peek() == 'X'))
        {
            value.Append(CurrentChar());
            Advance();
            value.Append(CurrentChar());
            Advance();

            while (position.Absolute < source.Length && IsHexDigit(CurrentChar()))
            {
                value.Append(CurrentChar());
                Advance();
            }
        }
        else
        {
            while (position.Absolute < source.Length && char.IsDigit(CurrentChar()))
            {
                value.Append(CurrentChar());
                Advance();
            }

            if (position.Absolute < source.Length && CurrentChar() == '.')
            {
                value.Append(CurrentChar());
                Advance();

                while (position.Absolute < source.Length && char.IsDigit(CurrentChar()))
                {
                    value.Append(CurrentChar());
                    Advance();
                }
            }

            if (position.Absolute < source.Length && (CurrentChar() == 'e' || CurrentChar() == 'E'))
            {
                value.Append(CurrentChar());
                Advance();

                if (position.Absolute < source.Length && (CurrentChar() == '+' || CurrentChar() == '-'))
                {
                    value.Append(CurrentChar());
                    Advance();
                }

                while (position.Absolute < source.Length && char.IsDigit(CurrentChar()))
                {
                    value.Append(CurrentChar());
                    Advance();
                }
            }
        }

        return CreateToken(TokenType.NumberLiteral, value.ToString(), start);
    }

    private Token ReadString()
    {
        Position start = position.Clone();
        StringBuilder value = new StringBuilder();
        char quoteChar = CurrentChar();

        value.Append(CurrentChar());
        Advance();

        while (position.Absolute < source.Length && CurrentChar() != quoteChar)
        {
            if (CurrentChar() == '\\')
            {
                value.Append(CurrentChar());
                Advance();
                if (position.Absolute < source.Length)
                {
                    value.Append(CurrentChar());
                    Advance();
                }
            }
            else
            {
                value.Append(CurrentChar());
                Advance();
            }
        }

        if (position.Absolute < source.Length && CurrentChar() == quoteChar)
        {
            value.Append(CurrentChar());
            Advance();
        }

        return CreateToken(TokenType.StringLiteral, value.ToString(), start);
    }

    private Token ReadMultiLineString()
    {
        Position start = position.Clone();
        StringBuilder value = new StringBuilder();

        // Read opening quotes
        for (int i = 0; i < 3; i++)
        {
            value.Append(CurrentChar());
            Advance();
        }

        while (position.Absolute < source.Length)
        {
            // Check for closing quotes
            if (CurrentChar() == '"' && Peek() == '"' && Peek(2) == '"')
            {
                for (int i = 0; i < 3; i++)
                {
                    value.Append(CurrentChar());
                    Advance();
                }
                break;
            }

            value.Append(CurrentChar());
            Advance();
        }

        return CreateToken(TokenType.StringLiteral, value.ToString(), start);
    }

    private Token ReadPlusOperator()
    {
        Position start = position.Clone();
        if (Peek() == '=')
        {
            Advance();
            Advance();
            return CreateToken(TokenType.PlusAssign, "+=", start);
        }
        Advance();
        return CreateToken(TokenType.Plus, "+", start);
    }

    private Token ReadMinusOperator()
    {
        Position start = position.Clone();
        if (Peek() == '=')
        {
            Advance();
            Advance();
            return CreateToken(TokenType.MinusAssign, "-=", start);
        }
        if (Peek() == '>')
        {
            Advance();
            Advance();
            return CreateToken(TokenType.ReturnType, "->", start);
        }
        Advance();
        return CreateToken(TokenType.Minus, "-", start);
    }

    private Token ReadMultiplyOperator()
    {
        Position start = position.Clone();
        if (Peek() == '*')
        {
            Advance();
            Advance();
            return CreateToken(TokenType.Power, "**", start);
        }
        if (Peek() == '=')
        {
            Advance();
            Advance();
            return CreateToken(TokenType.MultiplyAssign, "*=", start);
        }
        Advance();
        return CreateToken(TokenType.Multiply, "*", start);
    }

    private Token ReadDivideOperator()
    {
        Position start = position.Clone();
        if (Peek() == '=')
        {
            Advance();
            Advance();
            return CreateToken(TokenType.DivideAssign, "/=", start);
        }
        Advance();
        return CreateToken(TokenType.Divide, "/", start);
    }

    private Token ReadModuloOperator()
    {
        Position start = position.Clone();
        if (Peek() == '=')
        {
            Advance();
            Advance();
            return CreateToken(TokenType.ModuloAssign, "%=", start);
        }
        Advance();
        return CreateToken(TokenType.Modulo, "%", start);
    }

    private Token ReadEqualOperator()
    {
        Position start = position.Clone();
        if (Peek() == '=')
        {
            Advance();
            Advance();
            return CreateToken(TokenType.Equal, "==", start);
        }
        Advance();
        return CreateToken(TokenType.Assign, "=", start);
    }

    private Token ReadNotEqualOperator()
    {
        Position start = position.Clone();
        if (Peek() == '=')
        {
            Advance();
            Advance();
            return CreateToken(TokenType.NotEqual, "!=", start);
        }
        Advance();
        return CreateToken(TokenType.Error, "!");
    }

    private Token ReadLessOperator()
    {
        Position start = position.Clone();
        if (Peek() == '=')
        {
            Advance();
            Advance();
            return CreateToken(TokenType.LessEqual, "<=", start);
        }
        Advance();
        return CreateToken(TokenType.Less, "<", start);
    }

    private Token ReadGreaterOperator()
    {
        Position start = position.Clone();
        if (Peek() == '=')
        {
            Advance();
            Advance();
            return CreateToken(TokenType.GreaterEqual, ">=", start);
        }
        Advance();
        return CreateToken(TokenType.Greater, ">", start);
    }

    private void SkipSingleLineComment()
    {
        while (position.Absolute < source.Length && CurrentChar() != '\n')
        {
            Advance();
        }
        // Skip the newline character
        if (position.Absolute < source.Length && CurrentChar() == '\n')
        {
            Advance();
        }
    }

    private void SkipMultiLineComment()
    {
        // Skip "/*"
        Advance();
        Advance();

        while (position.Absolute < source.Length)
        {
            if (CurrentChar() == '*' && Peek() == '/')
            {
                Advance(); // Skip '*'
                Advance(); // Skip '/'
                break;
            }
            Advance();
        }
    }

    private void SkipWhitespace()
    {
        while (position.Absolute < source.Length && char.IsWhiteSpace(CurrentChar()))
        {
            Advance();
        }
    }

    private char CurrentChar()
    {
        return position.Absolute < source.Length ? source[position.Absolute] : '\0';
    }

    private char Peek(int offset = 1)
    {
        int peekPosition = position.Absolute + offset;
        return peekPosition < source.Length ? source[peekPosition] : '\0';
    }

    private void Advance()
    {
        if (position.Absolute < source.Length)
        {
            position.Advance(CurrentChar());
        }
    }

    private Token CreateToken(TokenType type, string value)
    {
        return new Token(type, value, position.Line, position.Column, position.Absolute);
    }

    private Token CreateToken(TokenType type, string value, Position start)
    {
        return new Token(type, value, start.Line, start.Column, start.Absolute);
    }

    private bool IsHexDigit(char c)
    {
        return char.IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
    }
}