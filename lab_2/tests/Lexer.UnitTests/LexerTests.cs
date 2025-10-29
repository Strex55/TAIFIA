using Lexer;

namespace Lexer.UnitTests;

public class LexerTests
{
    [Fact]
    public void EmptyInput_ReturnsOnlyEOF()
    {
        // Arrange
        var lexer = new Lexer("");
        
        // Act
        var tokens = lexer.Tokenize().ToList();
        
        // Assert
        Assert.Single(tokens);
        Assert.Equal(TokenType.EOF, tokens[0].Type);
    }

    [Fact]
    public void SingleSemicolon_ReturnsSemicolonAndEOF()
    {
        // Arrange
        var lexer = new Lexer(";");
        
        // Act
        var tokens = lexer.Tokenize().ToList();
        
        // Assert
        Assert.Equal(2, tokens.Count);
        Assert.Equal(TokenType.SEMICOLON, tokens[0].Type);
        Assert.Equal(";", tokens[0].Value);
        Assert.Equal(TokenType.EOF, tokens[1].Type);
    }
}