using Xunit;

public class ReverseTests
{
    [Fact]
    public void Reverse_SimpleWord()
    {
        Assert.Equal("olleH", StringReverser.Reverse("Hello"));
    }

    [Fact]
    public void Reverse_EmptyString_ReturnsEmpty()
    {
        Assert.Equal("", StringReverser.Reverse(""));
    }

    [Fact]
    public void Reverse_Whitespace()
    {
        Assert.Equal("   ", StringReverser.Reverse("   "));
    }

    [Fact]
    public void Reverse_Palindrome()
    {
        Assert.Equal("level", StringReverser.Reverse("level"));
    }

    [Fact]
    public void Reverse_WithSpacesInside()
    {
        Assert.Equal("dlroW olleH", StringReverser.Reverse("Hello World"));
    }

    [Fact]
    public void Reverse_Null_ReturnsEmpty()
    {
        Assert.Equal("", StringReverser.Reverse(null));
    }

    [Fact]
    public void Reverse_UnicodeCharacters()
    {
        Assert.Equal("界世 ,olleH", StringReverser.Reverse("Hello, 世界"));
    }

    [Fact]
    public void Reverse_LongString()
    {
        string original = new string('A', 5000) + new string('B', 5000);
        string expected = new string('B', 5000) + new string('A', 5000);

        Assert.Equal(expected, StringReverser.Reverse(original));
    }
}
