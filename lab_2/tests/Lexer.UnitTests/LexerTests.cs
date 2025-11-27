using Astra.Lexer;
using Xunit;

namespace Lexer.UnitTests;

public class LexerTests
{
    [Fact]
    public void EmptyInput_ReturnsEndOfFile()
    {
        // Arrange
        var lexer = new Lexer("");
        
        // Act
        var token = lexer.NextToken();
        
        // Assert
        Assert.Equal(TokenType.EndOfFile, token.Type);
        Assert.Equal("", token.Value);
    }
    
    [Fact]
    public void OnlyWhitespace_ReturnsEndOfFile()
    {
        // Arrange
        var lexer = new Lexer("   \t\n  \r\n  ");
        
        // Act
        var token = lexer.NextToken();
        
        // Assert
        Assert.Equal(TokenType.EndOfFile, token.Type);
    }
}