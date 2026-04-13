using Xunit;

namespace ExampleLib.UnitTests;

public class TextUtilTest
{
    [Fact]
    public void Can_extract_russian_words()
    {
        const string text = """
                            Играют волны — ветер свищет,
                            И мачта гнётся и скрыпит…
                            Увы! он счастия не ищет
                            И не от счастия бежит!
                            """;
        List<string> expected =
        [
            "Играют",
            "волны",
            "ветер",
            "свищет",
            "И",
            "мачта",
            "гнётся",
            "и",
            "скрыпит",
            "Увы",
            "он",
            "счастия",
            "не",
            "ищет",
            "И",
            "не",
            "от",
            "счастия",
            "бежит",
        ];

        List<string> actual = TextUtil.ExtractWords(text);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_extract_words_with_hyphens()
    {
        const string text = "Что-нибудь да как-нибудь, и +/- что- то ещё";
        List<string> expected =
        [
            "Что-нибудь",
            "да",
            "как-нибудь",
            "и",
            "что",
            "то",
            "ещё",
        ];

        List<string> actual = TextUtil.ExtractWords(text);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_extract_words_with_apostrophes()
    {
        const string text = "Children's toys and three cats' toys";
        List<string> expected =
        [
            "Children's",
            "toys",
            "and",
            "three",
            "cats'",
            "toys",
        ];

        List<string> actual = TextUtil.ExtractWords(text);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_extract_words_with_grave_accent()
    {
        const string text = "Children`s toys and three cats` toys, all of''them are green";
        List<string> expected =
        [
            "Children`s",
            "toys",
            "and",
            "three",
            "cats`",
            "toys",
            "all",
            "of'",
            "them",
            "are",
            "green",
        ];

        List<string> actual = TextUtil.ExtractWords(text);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_parse_basic_roman_numbers()
    {
        string[] romans = ["I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X"];
        int[] expected = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        for (int i = 0; i < romans.Length; i++)
        {
            int actual = TextUtil.ParseRoman(romans[i]);
            Assert.Equal(expected[i], actual);
        }
    }

    [Fact]
    public void Can_parse_complex_roman_numbers()
    {
        string[] romans = ["XL", "XC", "CD", "CM", "MCMXLIV", "MMXXV", "MMM"];
        int[] expected = [40, 90, 400, 900, 1944, 2025, 3000];

        for (int i = 0; i < romans.Length; i++)
        {
            int actual = TextUtil.ParseRoman(romans[i]);
            Assert.Equal(expected[i], actual);
        }
    }

    [Fact]
    public void Empty_string_returns_zero()
    {
        int actual = TextUtil.ParseRoman("");
        Assert.Equal(0, actual);
    }

    [Fact]
    public void Null_throws_argument_null_exception()
    {
        Assert.Throws<ArgumentNullException>(() => TextUtil.ParseRoman(null!));
    }

    [Fact]
    public void Invalid_roman_numbers_throw_argument_exception()
    {
        string[] invalids = ["IIII", "VV", "IL", "IC", "IIV", "XM", "ABC"];
        foreach (string s in invalids)
        {
            Assert.Throws<ArgumentException>(() => TextUtil.ParseRoman(s));
        }
    }

    [Fact]
    public void Too_large_numbers_throw_argument_exception()
    {
        Assert.Throws<ArgumentException>(() => TextUtil.ParseRoman("MMMM"));
    }
}