using Astra.Lexer;
using Xunit;

namespace Lexer.UnitTests;

public class LexicalStatsTests
{
    [Fact]
    public void CollectFromFile_SimpleProgram_ReturnsCorrectStats()
    {
        // Arrange
        string tempFile = Path.GetTempFileName();
        try
        {
            string program = """
                namespace math

                func add(a: number, b: number) -> number
                    return a + b
                end

                start
                    let result: number = add(5, 7)
                    show("Result:", result)
                end
                """;

            File.WriteAllText(tempFile, program);

            string expected = NormalizeNewLines("""
                keywords: 8
                identifiers: 13
                number literals: 2
                string literals: 1
                operators: 3
                other lexemes: 12
                """);

            // Act
            string result = LexicalStats.CollectFromFile(tempFile);

            // Assert
            Assert.Equal(expected, NormalizeNewLines(result));
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public void CollectFromFile_ComplexProgram_ReturnsCorrectStats()
    {
        // Arrange
        string tempFile = Path.GetTempFileName();
        try
        {
            string program = """
                start
                    # Программа для вычисления факториала
                    let n: number = 5
                    let factorial: number = 1
                    
                    for i in 1..n
                        factorial = factorial * i
                    end
                    
                    if factorial > 10 and n >= 3
                        show("Factorial of", n, "is", factorial)
                    else
                        show("Too small")
                    end
                end
                """;

            File.WriteAllText(tempFile, program);

            string expected = NormalizeNewLines("""
                keywords: 13
                identifiers: 13
                number literals: 5
                string literals: 3
                operators: 6
                other lexemes: 9
                """);

            // Act
            string result = LexicalStats.CollectFromFile(tempFile);

            // Assert
            Assert.Equal(expected, NormalizeNewLines(result));
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public void CollectFromFile_FileNotFound_ThrowsException()
    {
        // Arrange
        string nonExistentFile = Path.Combine(Path.GetTempPath(), "nonexistent.astra");

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() =>
            LexicalStats.CollectFromFile(nonExistentFile));
    }

    [Fact]
    public void CollectFromFile_EmptyFile_ReturnsZeroStats()
    {
        // Arrange
        string tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFile, "");

            string expected = NormalizeNewLines("""
                keywords: 0
                identifiers: 0
                number literals: 0
                string literals: 0
                operators: 0
                other lexemes: 0
                """);

            // Act
            string result = LexicalStats.CollectFromFile(tempFile);

            // Assert
            Assert.Equal(expected, NormalizeNewLines(result));
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public void CollectFromFile_OnlyComments_ReturnsZeroStats()
    {
        // Arrange
        string tempFile = Path.GetTempFileName();
        try
        {
            string program = """
                # Это комментарий
                /* 
                   Многострочный 
                   комментарий 
                */
                """;

            File.WriteAllText(tempFile, program);

            string expected = NormalizeNewLines("""
                keywords: 0
                identifiers: 0
                number literals: 0
                string literals: 0
                operators: 0
                other lexemes: 0
                """);

            // Act
            string result = LexicalStats.CollectFromFile(tempFile);

            // Assert
            Assert.Equal(expected, NormalizeNewLines(result));
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    private string NormalizeNewLines(string input)
    {
        return input.Replace("\r\n", "\n").Replace("\r", "\n").Trim();
    }
}