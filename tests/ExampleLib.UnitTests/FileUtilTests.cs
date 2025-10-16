using ExampleLib.UnitTests.Helpers;
using System;
using System.IO;
using Xunit;

namespace ExampleLib.UnitTests;

public class FileUtilTests
{
    [Fact]
    public void CanSortTextFile()
    {
        const string unsorted = """
                                Играют волны — ветер свищет,
                                И мачта гнется и скрыпит…
                                Увы! он счастия не ищет
                                И не от счастия бежит!
                                """;
        const string sorted = """
                              И мачта гнется и скрыпит…
                              И не от счастия бежит!
                              Играют волны — ветер свищет,
                              Увы! он счастия не ищет
                              """;

        using TempFile file = TempFile.Create(unsorted);
        FileUtil.SortFileLines(file.Path);

        string actual = File.ReadAllText(file.Path);
        Assert.Equal(sorted.Replace("\r\n", "\n"), actual);
    }

    [Fact]
    public void CanSortOneLineFile()
    {
        using TempFile file = TempFile.Create("Играют волны — ветер свищет,");
        FileUtil.SortFileLines(file.Path);

        string actual = File.ReadAllText(file.Path);
        Assert.Equal("Играют волны — ветер свищет,", actual);
    }

    [Fact]
    public void CanSortEmptyFile()
    {
        using TempFile file = TempFile.Create("");

        FileUtil.SortFileLines(file.Path);

        string actual = File.ReadAllText(file.Path);
        Assert.Equal("", actual);
    }

    // Новые тесты для метода AddLineNumbers
    [Fact]
    public void AddLineNumbers_ValidFile_AddsLineNumbersCorrectly()
    {
        // Arrange
        const string content = """
                               First line
                               Second line
                               Third line
                               """;
        using TempFile file = TempFile.Create(content);

        // Act
        FileUtil.AddLineNumbers(file.Path);

        // Assert
        string[] result = File.ReadAllLines(file.Path);
        Assert.Equal(3, result.Length);
        Assert.Equal("1. First line", result[0]);
        Assert.Equal("2. Second line", result[1]);
        Assert.Equal("3. Third line", result[2]);
    }

    [Fact]
    public void AddLineNumbers_EmptyFile_RemainsEmpty()
    {
        // Arrange
        using TempFile file = TempFile.Create("");

        // Act
        FileUtil.AddLineNumbers(file.Path);

        // Assert
        string result = File.ReadAllText(file.Path);
        Assert.Equal("", result);
    }

    [Fact]
    public void AddLineNumbers_SingleLine_AddsLineNumber()
    {
        // Arrange
        using TempFile file = TempFile.Create("Hello World");

        // Act
        FileUtil.AddLineNumbers(file.Path);

        // Assert
        string result = File.ReadAllText(file.Path).Trim();
        Assert.Equal("1. Hello World", result);
    }

    [Fact]
    public void AddLineNumbers_FileWithEmptyLines_NumbersAllLines()
    {
        // Arrange
        const string content = """
                               Line 1

                               Line 3
                                  
                               """;
        using TempFile file = TempFile.Create(content);

        // Act
        FileUtil.AddLineNumbers(file.Path);

        // Assert
        string[] result = File.ReadAllLines(file.Path);
        Assert.Equal("1. Line 1", result[0]);
        Assert.Equal("2. ", result[1]);
        Assert.Equal("3. Line 3", result[2]);
        Assert.Equal("4.    ", result[3]);
    }

    [Fact]
    public void AddLineNumbers_NullPath_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => FileUtil.AddLineNumbers(null));
    }

    [Fact]
    public void AddLineNumbers_EmptyPath_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => FileUtil.AddLineNumbers(""));
    }

    [Fact]
    public void AddLineNumbers_WhitespacePath_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => FileUtil.AddLineNumbers("   "));
    }

    [Fact]
    public void AddLineNumbers_NonExistentFile_ThrowsFileNotFoundException()
    {
        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => FileUtil.AddLineNumbers("nonexistent.txt"));
    }
}