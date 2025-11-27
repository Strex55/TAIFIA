using Astra.Lexer;
using Xunit;

namespace Lexer.UnitTests;

public class LexerTests
{
    [Fact]
    public void EmptyInput_ReturnsEndOfFile()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.EndOfFile, token.Type);
        Assert.Equal("", token.Value);
    }
    
    [Fact]
    public void OnlyWhitespace_ReturnsEndOfFile()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("   \t\n  \r\n  ");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.EndOfFile, token.Type);
    }

    [Fact]
    public void SingleLineComment_SkipsComment()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("# это комментарий\nstart");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.Start, token.Type);
        Assert.Equal("start", token.Value);
    }

    [Fact]
    public void MultiLineComment_SkipsComment()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("/* многострочный\nкомментарий */end");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.End, token.Type);
        Assert.Equal("end", token.Value);
    }

    [Fact]
    public void OnlyComments_ReturnsEndOfFile()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("# комментарий\n/* еще комментарий */");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.EndOfFile, token.Type);
    }

    [Fact]
    public void StartKeyword_ReturnsStartToken()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("start");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.Start, token.Type);
        Assert.Equal("start", token.Value);
    }

    [Fact]
    public void EndKeyword_ReturnsEndToken()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("end");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.End, token.Type);
        Assert.Equal("end", token.Value);
    }

    [Fact]
    public void KeywordsCaseInsensitive_ReturnsCorrectTokens()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("Start END nAmEsPaCe");
        Token token1 = lexer.NextToken();
        Assert.Equal(TokenType.Start, token1.Type);
        Assert.Equal("Start", token1.Value);
        
        Token token2 = lexer.NextToken();
        Assert.Equal(TokenType.End, token2.Type);
        Assert.Equal("END", token2.Value);
        
        Token token3 = lexer.NextToken();
        Assert.Equal(TokenType.Namespace, token3.Type);
        Assert.Equal("nAmEsPaCe", token3.Value);
    }

    [Fact]
    public void Semicolon_ReturnsSemicolonToken()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer(";");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.Semicolon, token.Type);
        Assert.Equal(";", token.Value);
    }

    [Fact]
    public void Identifier_ReturnsIdentifierToken()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("variableName");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("variableName", token.Value);
    }

    [Fact]
    public void IdentifierWithUnderscore_ReturnsIdentifierToken()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("_private_var");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("_private_var", token.Value);
    }

    [Fact]
    public void IntegerLiteral_ReturnsNumberToken()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("42");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.NumberLiteral, token.Type);
        Assert.Equal("42", token.Value);
    }

    [Fact]
    public void NegativeNumber_ReturnsNumberToken()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("-15");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.NumberLiteral, token.Type);
        Assert.Equal("-15", token.Value);
    }

    [Fact]
    public void FloatLiteral_ReturnsNumberToken()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("3.14");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.NumberLiteral, token.Type);
        Assert.Equal("3.14", token.Value);
    }

    [Fact]
    public void ScientificNotation_ReturnsNumberToken()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("1e6");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.NumberLiteral, token.Type);
        Assert.Equal("1e6", token.Value);
    }

    [Fact]
    public void HexLiteral_ReturnsNumberToken()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("0xFF");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.NumberLiteral, token.Type);
        Assert.Equal("0xFF", token.Value);
    }

    [Fact]
    public void StringLiteralDoubleQuotes_ReturnsStringToken()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("\"Hello\"");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.StringLiteral, token.Type);
        Assert.Equal("\"Hello\"", token.Value);
    }

    [Fact]
    public void StringLiteralSingleQuotes_ReturnsStringToken()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("'World'");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.StringLiteral, token.Type);
        Assert.Equal("'World'", token.Value);
    }

    [Fact]
    public void MultiLineString_ReturnsStringToken()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("\"\"\"Line1\nLine2\"\"\"");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.StringLiteral, token.Type);
        Assert.Equal("\"\"\"Line1\nLine2\"\"\"", token.Value);
    }

    [Fact]
    public void StringWithEscapes_ReturnsStringToken()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer(@"""Line1\nLine2\tTab""");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.StringLiteral, token.Type);
        Assert.Equal(@"""Line1\nLine2\tTab""", token.Value);
    }

    [Fact]
    public void BooleanLiterals_ReturnsBooleanTokens()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("true false");
        Token token1 = lexer.NextToken();
        Assert.Equal(TokenType.True, token1.Type);
        Token token2 = lexer.NextToken();
        Assert.Equal(TokenType.False, token2.Type);
    }

    [Fact]
    public void NullLiteral_ReturnsNullToken()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("null");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.Null, token.Type);
    }

    [Fact]
    public void ArithmeticOperators_ReturnsCorrectTokens()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("+ - * / % **");
        Assert.Equal(TokenType.Plus, lexer.NextToken().Type);
        Assert.Equal(TokenType.Minus, lexer.NextToken().Type);
        Assert.Equal(TokenType.Multiply, lexer.NextToken().Type);
        Assert.Equal(TokenType.Divide, lexer.NextToken().Type);
        Assert.Equal(TokenType.Modulo, lexer.NextToken().Type);
        Assert.Equal(TokenType.Power, lexer.NextToken().Type);
    }

    [Fact]
    public void ComparisonOperators_ReturnsCorrectTokens()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("== != < <= > >=");
        Assert.Equal(TokenType.Equal, lexer.NextToken().Type);
        Assert.Equal(TokenType.NotEqual, lexer.NextToken().Type);
        Assert.Equal(TokenType.Less, lexer.NextToken().Type);
        Assert.Equal(TokenType.LessEqual, lexer.NextToken().Type);
        Assert.Equal(TokenType.Greater, lexer.NextToken().Type);
        Assert.Equal(TokenType.GreaterEqual, lexer.NextToken().Type);
    }

    [Fact]
    public void AssignmentOperators_ReturnsCorrectTokens()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("= += -= *= /= %=");
        Assert.Equal(TokenType.Assign, lexer.NextToken().Type);
        Assert.Equal(TokenType.PlusAssign, lexer.NextToken().Type);
        Assert.Equal(TokenType.MinusAssign, lexer.NextToken().Type);
        Assert.Equal(TokenType.MultiplyAssign, lexer.NextToken().Type);
        Assert.Equal(TokenType.DivideAssign, lexer.NextToken().Type);
        Assert.Equal(TokenType.ModuloAssign, lexer.NextToken().Type);
    }

    [Fact]
    public void ReturnTypeOperator_ReturnsReturnTypeToken()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("->");
        Token token = lexer.NextToken();
        Assert.Equal(TokenType.ReturnType, token.Type);
        Assert.Equal("->", token.Value);
    }

    [Fact]
    public void Delimiters_ReturnsCorrectTokens()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer(", : ; ( ) [ ] { }");
        Assert.Equal(TokenType.Comma, lexer.NextToken().Type);
        Assert.Equal(TokenType.Colon, lexer.NextToken().Type);
        Assert.Equal(TokenType.Semicolon, lexer.NextToken().Type);
        Assert.Equal(TokenType.LeftParen, lexer.NextToken().Type);
        Assert.Equal(TokenType.RightParen, lexer.NextToken().Type);
        Assert.Equal(TokenType.LeftBracket, lexer.NextToken().Type);
        Assert.Equal(TokenType.RightBracket, lexer.NextToken().Type);
        Assert.Equal(TokenType.LeftBrace, lexer.NextToken().Type);
        Assert.Equal(TokenType.RightBrace, lexer.NextToken().Type);
    }

    [Fact]
    public void LogicalOperators_ReturnsCorrectTokens()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("and or not");
        Assert.Equal(TokenType.And, lexer.NextToken().Type);
        Assert.Equal(TokenType.Or, lexer.NextToken().Type);
        Assert.Equal(TokenType.Not, lexer.NextToken().Type);
    }

    [Fact]
    public void ComplexExpression_ReturnsCorrectTokens()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("let result: number = (a + b) * 2;");
        Assert.Equal(TokenType.Let, lexer.NextToken().Type);
        Assert.Equal(TokenType.Identifier, lexer.NextToken().Type);
        Assert.Equal(TokenType.Colon, lexer.NextToken().Type);
        Assert.Equal(TokenType.Identifier, lexer.NextToken().Type);
        Assert.Equal(TokenType.Assign, lexer.NextToken().Type);
        Assert.Equal(TokenType.LeftParen, lexer.NextToken().Type);
        Assert.Equal(TokenType.Identifier, lexer.NextToken().Type);
        Assert.Equal(TokenType.Plus, lexer.NextToken().Type);
        Assert.Equal(TokenType.Identifier, lexer.NextToken().Type);
        Assert.Equal(TokenType.RightParen, lexer.NextToken().Type);
        Assert.Equal(TokenType.Multiply, lexer.NextToken().Type);
        Assert.Equal(TokenType.NumberLiteral, lexer.NextToken().Type);
        Assert.Equal(TokenType.Semicolon, lexer.NextToken().Type);
    }

    [Fact]
    public void FunctionDeclaration_ReturnsCorrectTokens()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("func add(a: number, b: number) -> number");
        Assert.Equal(TokenType.Func, lexer.NextToken().Type);
        Assert.Equal(TokenType.Identifier, lexer.NextToken().Type);
        Assert.Equal(TokenType.LeftParen, lexer.NextToken().Type);
        Assert.Equal(TokenType.Identifier, lexer.NextToken().Type);
        Assert.Equal(TokenType.Colon, lexer.NextToken().Type);
        Assert.Equal(TokenType.Identifier, lexer.NextToken().Type);
        Assert.Equal(TokenType.Comma, lexer.NextToken().Type);
        Assert.Equal(TokenType.Identifier, lexer.NextToken().Type);
        Assert.Equal(TokenType.Colon, lexer.NextToken().Type);
        Assert.Equal(TokenType.Identifier, lexer.NextToken().Type);
        Assert.Equal(TokenType.RightParen, lexer.NextToken().Type);
        Assert.Equal(TokenType.ReturnType, lexer.NextToken().Type);
        Assert.Equal(TokenType.Identifier, lexer.NextToken().Type);
    }

    [Fact]
    public void FullExampleFromSpecification_ReturnsCorrectTokens()
    {
        string code = """
            namespace math

            func add(a: number, b: number) -> number
                return a + b
            end

            start
                let result: number = add(5, 7)
                show("Result:", result)
            end
            """;
        
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer(code);
        List<Token> tokens = new List<Token>();
        
        Token token;
        while ((token = lexer.NextToken()).Type != TokenType.EndOfFile)
        {
            tokens.Add(token);
        }
        
        TokenType[] tokenTypes = tokens.Select(t => t.Type).ToArray();
        
        Assert.Contains(TokenType.Namespace, tokenTypes);
        Assert.Contains(TokenType.Identifier, tokenTypes);
        Assert.Contains(TokenType.Func, tokenTypes);
        Assert.Contains(TokenType.Identifier, tokenTypes);
        Assert.Contains(TokenType.Return, tokenTypes);
        Assert.Contains(TokenType.Start, tokenTypes);
        Assert.Contains(TokenType.Let, tokenTypes);
        Assert.Contains(TokenType.NumberLiteral, tokenTypes);
        Assert.Contains(TokenType.Show, tokenTypes);
        Assert.Contains(TokenType.StringLiteral, tokenTypes);
        Assert.Contains(TokenType.End, tokenTypes);
    }

    [Fact]
    public void ErrorToken_ForUnknownCharacters()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("let x = @;");
        Assert.Equal(TokenType.Let, lexer.NextToken().Type);
        Assert.Equal(TokenType.Identifier, lexer.NextToken().Type);
        Assert.Equal(TokenType.Assign, lexer.NextToken().Type);
        Assert.Equal(TokenType.Error, lexer.NextToken().Type);
        Assert.Equal(TokenType.Semicolon, lexer.NextToken().Type);
    }

    // [Fact]
    // public void PositionTracking_IsCorrect()
    // {
    //     Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("let x = 5;\nshow(x);");
    //     Token token1 = lexer.NextToken();
    //     Assert.Equal(1, token1.Line);
    //     Assert.Equal(1, token1.Column);
        
    //     Token token2 = lexer.NextToken();
    //     Assert.Equal(1, token2.Line);
    //     Assert.Equal(5, token2.Column);
        
    //     Token token3 = lexer.NextToken();
    //     Assert.Equal(1, token3.Line);
    //     Assert.Equal(7, token3.Column);
        
    //     Token token4 = lexer.NextToken();
    //     Assert.Equal(1, token4.Line);
    //     Assert.Equal(9, token4.Column);
        
    //     Token token5 = lexer.NextToken();
    //     Assert.Equal(1, token5.Line);
    //     Assert.Equal(10, token5.Column);
        
    //     Token token6 = lexer.NextToken();
    //     Assert.Equal(2, token6.Line);
    //     Assert.Equal(1, token6.Column);
    // }

    [Fact]
    public void MixedKeywordsAndIdentifiers_ReturnsCorrectTokens()
    {
        Astra.Lexer.Lexer lexer = new Astra.Lexer.Lexer("start let x = 5; end");
        Assert.Equal(TokenType.Start, lexer.NextToken().Type);
        Assert.Equal(TokenType.Let, lexer.NextToken().Type);
        Assert.Equal(TokenType.Identifier, lexer.NextToken().Type);
        Assert.Equal(TokenType.Assign, lexer.NextToken().Type);
        Assert.Equal(TokenType.NumberLiteral, lexer.NextToken().Type);
        Assert.Equal(TokenType.Semicolon, lexer.NextToken().Type);
        Assert.Equal(TokenType.End, lexer.NextToken().Type);
    }
}