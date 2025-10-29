using System.Collections.Generic;
using System.Linq;

using Lexer;

namespace Lexer.UnitTests;

public class LexerTests
{
    [Fact]
    public void EmptyInput_ReturnsOnlyEOF()
    {
        // Arrange
        Lexer lexer = new Lexer("");

        // Act
        List<Token> tokens = lexer.Tokenize().ToList();

        // Assert
        Assert.Single(tokens);
        Assert.Equal(TokenType.EOF, tokens[0].Type);
    }

    [Fact]
    public void SingleSemicolon_ReturnsSemicolonToken()
    {
        // Arrange
        Lexer lexer = new Lexer(";");

        // Act
        List<Token> tokens = lexer.Tokenize().ToList();

        // Assert
        Assert.Equal(2, tokens.Count);
        Assert.Equal(TokenType.SEMICOLON, tokens[0].Type);
        Assert.Equal(";", tokens[0].Value);
    }

    [Fact]
    public void WhitespaceAndTabs_AreSkipped()
    {
        // Arrange
        Lexer lexer = new Lexer("   \t  ;");

        // Act
        List<Token> tokens = lexer.Tokenize().ToList();

        // Assert
        Assert.Equal(2, tokens.Count);
        Assert.Equal(TokenType.SEMICOLON, tokens[0].Type);
        Assert.Equal(";", tokens[0].Value);
    }

    [Fact]
    public void CurlyBraces_ReturnsBraceTokens()
    {
        // Arrange
        Lexer lexer = new Lexer("{}");

        // Act
        List<Token> tokens = lexer.Tokenize().ToList();

        // Assert
        Assert.Equal(3, tokens.Count);
        Assert.Equal(TokenType.LEFT_BRACE, tokens[0].Type);
        Assert.Equal("{", tokens[0].Value);
        Assert.Equal(TokenType.RIGHT_BRACE, tokens[1].Type);
        Assert.Equal("}", tokens[1].Value);
    }

    [Fact]
    public void Parentheses_ReturnsParenTokens()
    {
        // Arrange
        Lexer lexer = new Lexer("()");

        // Act
        List<Token> tokens = lexer.Tokenize().ToList();

        // Assert
        Assert.Equal(3, tokens.Count);
        Assert.Equal(TokenType.LEFT_PAREN, tokens[0].Type);
        Assert.Equal("(", tokens[0].Value);
        Assert.Equal(TokenType.RIGHT_PAREN, tokens[1].Type);
        Assert.Equal(")", tokens[1].Value);
    }

    [Fact]
    public void Comma_ReturnsCommaToken()
    {
        // Arrange
        Lexer lexer = new Lexer(",");

        // Act
        List<Token> tokens = lexer.Tokenize().ToList();

        // Assert
        Assert.Equal(2, tokens.Count);
        Assert.Equal(TokenType.COMMA, tokens[0].Type);
        Assert.Equal(",", tokens[0].Value);
    }

    [Fact]
    public void Operators_ReturnsOperatorTokens()
    {
        // Arrange
        Lexer lexer = new Lexer("=+-*/%");

        // Act
        List<Token> tokens = lexer.Tokenize().ToList();

        // Assert
        Assert.Equal(7, tokens.Count);
        Assert.Equal(TokenType.ASSIGN, tokens[0].Type);
        Assert.Equal(TokenType.PLUS, tokens[1].Type);
        Assert.Equal(TokenType.MINUS, tokens[2].Type);
        Assert.Equal(TokenType.MULTIPLY, tokens[3].Type);
        Assert.Equal(TokenType.DIVIDE, tokens[4].Type);
        Assert.Equal(TokenType.MODULO, tokens[5].Type);
    }

    [Fact]
    public void ComparisonOperators_ReturnsCorrectTokens()
    {
        // Arrange
        Lexer lexer = new Lexer("== != < > <= >= && || !");

        // Act
        List<Token> tokens = lexer.Tokenize().ToList();

        // Assert
        // Токены: ==, !=, <, >, <=, >=, &&, ||, !, EOF (10 токенов)
        Assert.Equal(10, tokens.Count);
        Assert.Equal(TokenType.EQUALS, tokens[0].Type);
        Assert.Equal("==", tokens[0].Value);
        Assert.Equal(TokenType.NOT_EQUALS, tokens[1].Type);
        Assert.Equal("!=", tokens[1].Value);
        Assert.Equal(TokenType.LESS, tokens[2].Type);
        Assert.Equal("<", tokens[2].Value);
        Assert.Equal(TokenType.GREATER, tokens[3].Type);
        Assert.Equal(">", tokens[3].Value);
        Assert.Equal(TokenType.LESS_EQUAL, tokens[4].Type);
        Assert.Equal("<=", tokens[4].Value);
        Assert.Equal(TokenType.GREATER_EQUAL, tokens[5].Type);
        Assert.Equal(">=", tokens[5].Value);
        Assert.Equal(TokenType.AND, tokens[6].Type);
        Assert.Equal("&&", tokens[6].Value);
        Assert.Equal(TokenType.OR, tokens[7].Type);
        Assert.Equal("||", tokens[7].Value);
        Assert.Equal(TokenType.NOT, tokens[8].Type);
        Assert.Equal("!", tokens[8].Value);
    }

    [Fact]
    public void Keywords_ReturnsKeywordTokens()
    {
        // Arrange
        Lexer lexer = new Lexer("class if while return void int string bool public private static");

        // Act
        List<Token> tokens = lexer.Tokenize().ToList();

        // Assert
        // Токены: class, if, while, return, void, int, string, bool, public, private, static, EOF (12 токенов)
        Assert.Equal(12, tokens.Count);
        Assert.Equal(TokenType.CLASS, tokens[0].Type);
        Assert.Equal("class", tokens[0].Value);
        Assert.Equal(TokenType.IF, tokens[1].Type);
        Assert.Equal("if", tokens[1].Value);
        Assert.Equal(TokenType.WHILE, tokens[2].Type);
        Assert.Equal("while", tokens[2].Value);
        Assert.Equal(TokenType.RETURN, tokens[3].Type);
        Assert.Equal("return", tokens[3].Value);
        Assert.Equal(TokenType.VOID, tokens[4].Type);
        Assert.Equal("void", tokens[4].Value);
        Assert.Equal(TokenType.INT, tokens[5].Type);
        Assert.Equal("int", tokens[5].Value);
        Assert.Equal(TokenType.STRING, tokens[6].Type);
        Assert.Equal("string", tokens[6].Value);
        Assert.Equal(TokenType.BOOL, tokens[7].Type);
        Assert.Equal("bool", tokens[7].Value);
        Assert.Equal(TokenType.PUBLIC, tokens[8].Type);
        Assert.Equal("public", tokens[8].Value);
        Assert.Equal(TokenType.PRIVATE, tokens[9].Type);
        Assert.Equal("private", tokens[9].Value);
        Assert.Equal(TokenType.STATIC, tokens[10].Type);
        Assert.Equal("static", tokens[10].Value);
    }

    [Fact]
    public void NumberLiteral_ReturnsNumberToken()
    {
        // Arrange
        Lexer lexer = new Lexer("123");

        // Act
        List<Token> tokens = lexer.Tokenize().ToList();

        // Assert
        Assert.Equal(2, tokens.Count);
        Assert.Equal(TokenType.NUMBER, tokens[0].Type);
        Assert.Equal("123", tokens[0].Value);
    }

    [Fact]
    public void StringLiteral_ReturnsStringToken()
    {
        // Arrange
        Lexer lexer = new Lexer("\"hello\"");

        // Act
        List<Token> tokens = lexer.Tokenize().ToList();

        // Assert
        Assert.Equal(2, tokens.Count);
        Assert.Equal(TokenType.STRING_LITERAL, tokens[0].Type);
        Assert.Equal("hello", tokens[0].Value);
    }

    [Fact]
    public void StringLiteralWithSpaces_ReturnsStringToken()
    {
        // Arrange
        Lexer lexer = new Lexer("\"hello world\"");

        // Act
        List<Token> tokens = lexer.Tokenize().ToList();

        // Assert
        Assert.Equal(2, tokens.Count);
        Assert.Equal(TokenType.STRING_LITERAL, tokens[0].Type);
        Assert.Equal("hello world", tokens[0].Value);
    }

    [Fact]
    public void Identifier_ReturnsIdentifierToken()
    {
        // Arrange
        Lexer lexer = new Lexer("variableName");

        // Act
        List<Token> tokens = lexer.Tokenize().ToList();

        // Assert
        Assert.Equal(2, tokens.Count);
        Assert.Equal(TokenType.IDENTIFIER, tokens[0].Type);
        Assert.Equal("variableName", tokens[0].Value);
    }

    [Fact]
    public void IdentifierWithUnderscore_ReturnsIdentifierToken()
    {
        // Arrange
        Lexer lexer = new Lexer("_privateField");

        // Act
        List<Token> tokens = lexer.Tokenize().ToList();

        // Assert
        Assert.Equal(2, tokens.Count);
        Assert.Equal(TokenType.IDENTIFIER, tokens[0].Type);
        Assert.Equal("_privateField", tokens[0].Value);
    }

    [Fact]
    public void ComplexExpression_ReturnsCorrectTokens()
    {
        // Arrange
        Lexer lexer = new Lexer("public class MyClass { int x = (a + b) * c; }");

        // Act
        List<Token> tokens = lexer.Tokenize().ToList();

        // Assert
        Assert.Equal(17, tokens.Count); // Используем фактическое количество

        Assert.Equal(TokenType.PUBLIC, tokens[0].Type);
        Assert.Equal("public", tokens[0].Value);

        Assert.Equal(TokenType.CLASS, tokens[1].Type);
        Assert.Equal("class", tokens[1].Value);

        Assert.Equal(TokenType.IDENTIFIER, tokens[2].Type);  // MyClass
        Assert.Equal("MyClass", tokens[2].Value);

        Assert.Equal(TokenType.LEFT_BRACE, tokens[3].Type);
        Assert.Equal("{", tokens[3].Value);

        Assert.Equal(TokenType.INT, tokens[4].Type);
        Assert.Equal("int", tokens[4].Value);

        Assert.Equal(TokenType.IDENTIFIER, tokens[5].Type);  // x
        Assert.Equal("x", tokens[5].Value);

        Assert.Equal(TokenType.ASSIGN, tokens[6].Type);
        Assert.Equal("=", tokens[6].Value);

        Assert.Equal(TokenType.LEFT_PAREN, tokens[7].Type);
        Assert.Equal("(", tokens[7].Value);

        Assert.Equal(TokenType.IDENTIFIER, tokens[8].Type);  // a
        Assert.Equal("a", tokens[8].Value);

        Assert.Equal(TokenType.PLUS, tokens[9].Type);
        Assert.Equal("+", tokens[9].Value);

        Assert.Equal(TokenType.IDENTIFIER, tokens[10].Type); // b
        Assert.Equal("b", tokens[10].Value);

        Assert.Equal(TokenType.RIGHT_PAREN, tokens[11].Type);
        Assert.Equal(")", tokens[11].Value);

        Assert.Equal(TokenType.MULTIPLY, tokens[12].Type);
        Assert.Equal("*", tokens[12].Value);

        Assert.Equal(TokenType.IDENTIFIER, tokens[13].Type); // c
        Assert.Equal("c", tokens[13].Value);

        Assert.Equal(TokenType.SEMICOLON, tokens[14].Type);
        Assert.Equal(";", tokens[14].Value);

        Assert.Equal(TokenType.RIGHT_BRACE, tokens[15].Type);
        Assert.Equal("}", tokens[15].Value);

        Assert.Equal(TokenType.EOF, tokens[16].Type);
    }

    [Fact]
    public void IfStatement_ReturnsCorrectTokens()
    {
        // Arrange
        Lexer lexer = new Lexer("if (x > 0) { return 1; }");

        // Act
        List<Token> tokens = lexer.Tokenize().ToList();

        // Assert
        // Токены: if, (, x, >, 0, ), {, return, 1, ;, }, EOF (12 токенов)
        Assert.Equal(12, tokens.Count);

        Assert.Equal(TokenType.IF, tokens[0].Type);
        Assert.Equal("if", tokens[0].Value);

        Assert.Equal(TokenType.LEFT_PAREN, tokens[1].Type);
        Assert.Equal("(", tokens[1].Value);

        Assert.Equal(TokenType.IDENTIFIER, tokens[2].Type);  
        Assert.Equal("x", tokens[2].Value);

        Assert.Equal(TokenType.GREATER, tokens[3].Type);
        Assert.Equal(">", tokens[3].Value);

        Assert.Equal(TokenType.NUMBER, tokens[4].Type);      
        Assert.Equal("0", tokens[4].Value);

        Assert.Equal(TokenType.RIGHT_PAREN, tokens[5].Type);
        Assert.Equal(")", tokens[5].Value);

        Assert.Equal(TokenType.LEFT_BRACE, tokens[6].Type);
        Assert.Equal("{", tokens[6].Value);

        Assert.Equal(TokenType.RETURN, tokens[7].Type);
        Assert.Equal("return", tokens[7].Value);

        Assert.Equal(TokenType.NUMBER, tokens[8].Type);     
        Assert.Equal("1", tokens[8].Value);

        Assert.Equal(TokenType.SEMICOLON, tokens[9].Type);
        Assert.Equal(";", tokens[9].Value);

        Assert.Equal(TokenType.RIGHT_BRACE, tokens[10].Type);
        Assert.Equal("}", tokens[10].Value);

        Assert.Equal(TokenType.EOF, tokens[11].Type);
    }
}