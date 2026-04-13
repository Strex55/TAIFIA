using ExampleLib.UnitTests.Helpers;
using Lexer;
using Xunit;

namespace Lexer.UnitTests;

public class LexerTest
{
    [Theory]
    [MemberData(nameof(GetTokenizeData))]
    public void LexerTestTheory(string text, List<Token> expected)
    {
        List<Token> actual = Tokenize(text);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetLexicalStatsData))]
    public void LexicalStatsTestTheory(string code, string expected)
    {
        using TempFile file = TempFile.Create(code);
        string actual = LexicalStats.CollectFromFile(file.Path);
        Assert.Equal(Normalize(expected), Normalize(actual));
    }

    public static TheoryData<string, List<Token>> GetTokenizeData()
    {
        return new TheoryData<string, List<Token>>
        {
            {
                "bello!",
                new List<Token>
                {
                    new Token(TokenType.Bello, "bello!", 0),
                }
            },
            {
                "   bello!   ",
                new List<Token>
                {
                    new Token(TokenType.Bello, "bello!", 3),
                }
            },
            {
                "bello!\npoop x Banana naidu!",
                new List<Token>
                {
                    new Token(TokenType.Bello, "bello!", 0),
                    new Token(TokenType.Poop, "poop", 7),
                    new Token(TokenType.Identifier, "x", 12),
                    new Token(TokenType.Identifier, "Banana", 14),
                    new Token(TokenType.Naidu, "naidu!", 21),
                }
            },
            {
                "oca! stopa bi-do uh-oh again kemari aspetta poop bapple trusela tulalilloo ti amo guoleila tank yu boo-ya",
                new List<Token>
                {
                    new Token(TokenType.Oca, "oca!", 0),
                    new Token(TokenType.Stopa, "stopa", 5),
                    new Token(TokenType.BiDo, "bi-do", 11),
                    new Token(TokenType.UhOh, "uh-oh", 17),
                    new Token(TokenType.Again, "again", 23),
                    new Token(TokenType.Kemari, "kemari", 29),
                    new Token(TokenType.Aspetta, "aspetta", 36),
                    new Token(TokenType.Poop, "poop", 44),
                    new Token(TokenType.Bapple, "bapple", 49),
                    new Token(TokenType.Trusela, "trusela", 56),
                    new Token(TokenType.Tulalilloo, "tulalilloo", 64),
                    new Token(TokenType.Ti, "ti", 75),
                    new Token(TokenType.Amo, "amo", 78),
                    new Token(TokenType.Guoleila, "guoleila", 82),
                    new Token(TokenType.Tank, "tank", 91),
                    new Token(TokenType.Yu, "yu", 96),
                    new Token(TokenType.BooYa, "boo-ya", 99),
                }
            },
            {
                "score",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "score", 0),
                }
            },
            {
                "_temp",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "_temp", 0),
                }
            },
            {
                "level2",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "level2", 0),
                }
            },
            {
                "x y z",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "x", 0),
                    new Token(TokenType.Identifier, "y", 2),
                    new Token(TokenType.Identifier, "z", 4),
                }
            },
            {
                "Banana",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "Banana", 0),
                }
            },
            {
                "Papaya",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "Papaya", 0),
                }
            },
            {
                "Gelato",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "Gelato", 0),
                }
            },
            {
                "Spaghetti",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "Spaghetti", 0),
                }
            },
            {
                "123abc",
                new List<Token>
                {
                    new Token(TokenType.Error, "123abc", 0),
                }
            },
            {
                "42test",
                new List<Token>
                {
                    new Token(TokenType.Error, "42test", 0),
                }
            },
            {
                "42",
                new List<Token>
                {
                    new Token(TokenType.NumberLiteral, "42", 0),
                }
            },
            {
                "-7",
                new List<Token>
                {
                    new Token(TokenType.NumberLiteral, "-7", 0),
                }
            },
            {
                "3.14",
                new List<Token>
                {
                    new Token(TokenType.NumberLiteral, "3.14", 0),
                }
            },
            {
                "!Hello!",
                new List<Token>
                {
                    new Token(TokenType.StringLiteral, "!Hello!", 0),
                }
            },
            {
                "!Hello!!World!",
                new List<Token>
                {
                    new Token(TokenType.StringLiteral, "!Hello!!World!", 0),
                }
            },
            {
                "!Line1\\nLine2!",
                new List<Token>
                {
                    new Token(TokenType.StringLiteral, "!Line1\\nLine2!", 0),
                }
            },
            {
                "Da No",
                new List<Token>
                {
                    new Token(TokenType.Da, "Da", 0),
                    new Token(TokenType.No, "No", 3),
                }
            },
            {
                "da no",
                new List<Token>
                {
                    new Token(TokenType.Da, "da", 0),
                    new Token(TokenType.No, "no", 3),
                }
            },
            {
                "x lumai 5",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "x", 0),
                    new Token(TokenType.Operator, "lumai", 2),
                    new Token(TokenType.NumberLiteral, "5", 8),
                }
            },
            {
                "a melomo b",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "a", 0),
                    new Token(TokenType.Operator, "melomo", 2),
                    new Token(TokenType.Identifier, "b", 9),
                }
            },
            {
                "a flavuk b",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "a", 0),
                    new Token(TokenType.Operator, "flavuk", 2),
                    new Token(TokenType.Identifier, "b", 9),
                }
            },
            {
                "a dibotada b",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "a", 0),
                    new Token(TokenType.Operator, "dibotada", 2),
                    new Token(TokenType.Identifier, "b", 11),
                }
            },
            {
                "a poopaye b",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "a", 0),
                    new Token(TokenType.Operator, "poopaye", 2),
                    new Token(TokenType.Identifier, "b", 10),
                }
            },
            {
                "a pado b",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "a", 0),
                    new Token(TokenType.Operator, "pado", 2),
                    new Token(TokenType.Identifier, "b", 7),
                }
            },
            {
                "a beedo (b Banana c)",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "a", 0),
                    new Token(TokenType.Operator, "beedo", 2),
                    new Token(TokenType.Delimiter, "(", 8),
                    new Token(TokenType.Identifier, "b", 9),
                    new Token(TokenType.Identifier, "Banana", 11),
                    new Token(TokenType.Identifier, "c", 18),
                    new Token(TokenType.Delimiter, ")", 19),
                }
            },
            {
                "x con y",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "x", 0),
                    new Token(TokenType.Operator, "con", 2),
                    new Token(TokenType.Identifier, "y", 6),
                }
            },
            {
                "x la y",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "x", 0),
                    new Token(TokenType.Operator, "la", 2),
                    new Token(TokenType.Identifier, "y", 5),
                }
            },
            {
                "x looka too y",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "x", 0),
                    new Token(TokenType.Operator, "looka", 2),
                    new Token(TokenType.Operator, "too", 8),
                    new Token(TokenType.Identifier, "y", 12),
                }
            },
            {
                "makoroni flag",
                new List<Token>
                {
                    new Token(TokenType.Operator, "makoroni", 0),
                    new Token(TokenType.Identifier, "flag", 9),
                }
            },
            {
                "a tropa b",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "a", 0),
                    new Token(TokenType.Operator, "tropa", 2),
                    new Token(TokenType.Identifier, "b", 8),
                }
            },
            {
                "a bo-ca b",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "a", 0),
                    new Token(TokenType.Operator, "bo-ca", 2),
                    new Token(TokenType.Identifier, "b", 8),
                }
            },
            {
                "a melomo b dibotada c",
                new List<Token>
                {
                    new Token(TokenType.Identifier, "a", 0),
                    new Token(TokenType.Operator, "melomo", 2),
                    new Token(TokenType.Identifier, "b", 9),
                    new Token(TokenType.Operator, "dibotada", 11),
                    new Token(TokenType.Identifier, "c", 20),
                }
            },
            {
                "\"Hello \" loka name loka \"!\"",
                new List<Token>
                {
                    new Token(TokenType.StringLiteral, "\"Hello \"", 0),
                    new Token(TokenType.Loka, "loka", 9),
                    new Token(TokenType.Identifier, "name", 14),
                    new Token(TokenType.Loka, "loka", 19),
                    new Token(TokenType.StringLiteral, "\"!\"", 24),
                }
            },
            {
                "oca! stopa",
                new List<Token>
                {
                    new Token(TokenType.Oca, "oca!", 0),
                    new Token(TokenType.Stopa, "stopa", 5),
                }
            },
            {
                "// banana!",
                new List<Token>
                {
                    new Token(TokenType.Comment, "// banana!", 0),
                }
            },
            {
                "/* bello! */",
                new List<Token>
                {
                    new Token(TokenType.Comment, "/* bello! */", 0),
                }
            },
            {
                "guoleila (name) naidu!",
                new List<Token>
                {
                    new Token(TokenType.Guoleila, "guoleila", 0),
                    new Token(TokenType.Delimiter, "(", 9),
                    new Token(TokenType.Identifier, "name", 10),
                    new Token(TokenType.Delimiter, ")", 14),
                    new Token(TokenType.Naidu, "naidu!", 16),
                }
            },
            {
                "tulalilloo ti amo (!Hello!) naidu!",
                new List<Token>
                {
                    new Token(TokenType.Tulalilloo, "tulalilloo", 0),
                    new Token(TokenType.Ti, "ti", 11),
                    new Token(TokenType.Amo, "amo", 14),
                    new Token(TokenType.Delimiter, "(", 18),
                    new Token(TokenType.StringLiteral, "!Hello!", 19),
                    new Token(TokenType.Delimiter, ")", 26),
                    new Token(TokenType.Naidu, "naidu!", 28),
                }
            },
            {
                "tulalilloo ti amo (!Hi, ! loka name loka !)",
                new List<Token>
                {
                    new Token(TokenType.Tulalilloo, "tulalilloo", 0),
                    new Token(TokenType.Ti, "ti", 11),
                    new Token(TokenType.Amo, "amo", 14),
                    new Token(TokenType.Delimiter, "(", 18),
                    new Token(TokenType.StringLiteral, "!Hi, !", 19),
                    new Token(TokenType.Loka, "loka", 26),
                    new Token(TokenType.Identifier, "name", 31),
                    new Token(TokenType.Loka, "loka", 36),
                    new Token(TokenType.StringLiteral, "!", 41),
                    new Token(TokenType.Delimiter, ")", 42),
                }
            },
            {
                "tank yu result naidu!",
                new List<Token>
                {
                    new Token(TokenType.Tank, "tank", 0),
                    new Token(TokenType.Yu, "yu", 5),
                    new Token(TokenType.Identifier, "result", 8),
                    new Token(TokenType.Naidu, "naidu!", 15),
                }
            },
            {
                "boo-ya Naletuna naidu!",
                new List<Token>
                {
                    new Token(TokenType.BooYa, "boo-ya", 0),
                    new Token(TokenType.Identifier, "Naletuna", 7),
                    new Token(TokenType.Naidu, "naidu!", 16),
                }
            },
            {
                """
                bello!
                poop name Spaghetti naidu!
                guoleila (name) naidu!
                tulalilloo ti amo (!Hello, ! loka name loka !) naidu!
                """,
                new List<Token>
                {
                    new Token(TokenType.Bello, "bello!", 0),
                    new Token(TokenType.Poop, "poop", 7),
                    new Token(TokenType.Identifier, "name", 12),
                    new Token(TokenType.Identifier, "Spaghetti", 17),
                    new Token(TokenType.Naidu, "naidu!", 27),
                    new Token(TokenType.Guoleila, "guoleila", 34),
                    new Token(TokenType.Delimiter, "(", 43),
                    new Token(TokenType.Identifier, "name", 44),
                    new Token(TokenType.Delimiter, ")", 48),
                    new Token(TokenType.Naidu, "naidu!", 50),
                    new Token(TokenType.Tulalilloo, "tulalilloo", 57),
                    new Token(TokenType.Ti, "ti", 68),
                    new Token(TokenType.Amo, "amo", 71),
                    new Token(TokenType.Delimiter, "(", 75),
                    new Token(TokenType.StringLiteral, "!Hello, !", 76),
                    new Token(TokenType.Loka, "loka", 86),
                    new Token(TokenType.Identifier, "name", 91),
                    new Token(TokenType.Loka, "loka", 96),
                    new Token(TokenType.StringLiteral, "!", 101),
                    new Token(TokenType.Delimiter, ")", 102),
                    new Token(TokenType.Naidu, "naidu!", 104),
                }
            },
        };
    }

    public static TheoryData<string, string> GetLexicalStatsData()
    {
        return new TheoryData<string, string>
        {
            {
                """
                bello!
                poop name Banana naidu!
                guoleila (name) naidu!
                tulalilloo ti amo (!Hello!) naidu!
                """,
                """
                keywords: 8
                identifier: 1
                number literals: 0
                string literals: 1
                operators: 0
                other lexemes: 5
                """
            },
            {
                """
                bello!
                poop x Banana naidu!
                bi-do (x la 10)
                oca!
                  tulalilloo ti amo (!small!) naidu!
                stopa
                """,
                """
                keywords: 9
                identifier: 1
                number literals: 1
                string literals: 1
                operators: 1
                other lexemes: 5
                """
            },
            {
                """
                bello!
                poop flag Papaya naidu!
                bi-do (flag con Da)
                oca!
                    tulalilloo ti amo (!True!) naidu!
                stopa
                """,
                """
                keywords: 10
                identifier: 1
                number literals: 0
                string literals: 1
                operators: 1
                other lexemes: 5
                """
            },
        };
    }

    private static string Normalize(string s) =>
        string.Concat(s.Where(c => !char.IsWhiteSpace(c)));

    private static List<Token> Tokenize(string text)
    {
        Lexer lexer = new(text);
        IReadOnlyList<Token> tokens = lexer.Tokenize();
        return tokens
            .Where(t => t.Type != TokenType.Whitespace)
            .ToList();
    }
}
