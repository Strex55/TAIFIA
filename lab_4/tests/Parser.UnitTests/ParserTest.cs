using Execution;
using Parser;
using Xunit;

namespace Parser.UnitTests;

public class ParserTest
{
    /// <summary>
    /// Тестирует парсинг выражений с проверкой результата вычисления
    /// </summary>
    [Theory]
    [MemberData(nameof(GetExpressionTests))]
    public void ParseExpressionTest(string code, List<decimal> expected)
    {
        Parser parser = new(new TokenStream(""));
        decimal result = parser.EvaluateExpression(code);
        Assert.Equal(expected[0], result);
    }

    /// <summary>
    /// Тестирует парсинг программ с проверкой результатов выполнения
    /// </summary>
    [Theory]
    [MemberData(nameof(GetProgramTests))]
    public void ParseProgramTest(string code, List<decimal> expected)
    {
        FakeEnvironment env = new();
        Parser.ParseProgram(code, env);
        IReadOnlyList<decimal> actual = env.Results;

        for (int i = 0; i < Math.Min(expected.Count, actual.Count); i++)
        {
            Assert.Equal(expected[i], actual[i]);
        }

        if (expected.Count != actual.Count)
        {
            Assert.Fail($"Expected {expected.Count} results, but got {actual.Count}. Expected: [{string.Join(", ", expected)}], Actual: [{string.Join(", ", actual)}]");
        }
    }

    /// <summary>
    /// Тестирует выполнение программ с вводом/выводом через FakeEnvironment
    /// </summary>
    [Theory]
    [MemberData(nameof(GetIoExecutionTests))]
    public void ExecutesIoWithFakeEnvironment(string code, decimal[] inputs, decimal[] expectedOutputs)
    {
        FakeEnvironment env = new();
        foreach (decimal input in inputs)
        {
            env.AddInput(input);
        }

        Parser.ParseProgram(code, env);

        Assert.Equal(expectedOutputs.Length, env.Results.Count);
        for (int i = 0; i < expectedOutputs.Length; i++)
        {
            Assert.Equal(expectedOutputs[i], env.Results[i]);
        }
    }

    /// <summary>
    /// Тестирует обработку ошибок парсинга
    /// </summary>
    [Theory]
    [MemberData(nameof(GetErrorTests))]
    public void ParseErrorTest(string code)
    {
        if (code.Contains("bello!"))
        {
            Assert.Throws<InvalidOperationException>(() => Parser.ParseProgram(code));
        }
        else
        {
            Parser parser = new(new TokenStream(code));
            Assert.ThrowsAny<Exception>(() => parser.EvaluateExpression(code));
        }
    }

    /// <summary>
    /// Тестовые данные для выражений
    /// </summary>
    public static TheoryData<string, List<decimal>> GetExpressionTests()
    {
        return new TheoryData<string, List<decimal>>
        {
            // Числа и базовые операции
            { "1", [1m] },
            { "10 melomo 5 flavuk 2", [13m] },
            { "flavuk 10 melomo 5 flavuk flavuk 5", [0m] }, // -10 + 5 - (-5) = 0
            { "10 dibotada 5 poopaye 2", [25m] },
            { "10 pado 2", [0m] },
            { "10 pado 3", [1m] },
            { "1.128 melomo 8 flavuk 7.5", [1.628m] },
            { "2 beedo 5", [32m] },
            { "(2 melomo 3) poopaye 10", [0.5m] },
            { "(flavuk 2) beedo 10", [1024m] },
            { "flavuk 2 beedo 10", [-1024m] },

            // Логические операции
            { "da", [1m] },
            { "no", [0m] },
            { "5 con 5", [1m] },
            { "5 nocon 10", [1m] },
            { "5 la 10", [1m] },
            { "5 la con 5", [1m] },
            { "10 looka too 5", [1m] },
            { "10 looka too con 10", [1m] },
            { "da tropa no", [0m] },
            { "da bo-ca no", [1m] },
            { "makoroni da", [0m] },
            { "makoroni no", [1m] },

            // Приоритет операторов
            { "2 melomo 3 dibotada 4", [14m] },
            { "2 dibotada 3 beedo 2", [18m] },
            { "(2 melomo 3) dibotada 4", [20m] },
            { "flavuk 5 dibotada 3", [-15m] },
            { "da tropa no bo-ca da", [1m] },

            // Функции
            { "muak(flavuk 15)", [15m] },
            { "miniboss(5, 4)", [4m] },
            { "bigboss(5, 4)", [5m] },
            { "miniboss(bigboss(1, 5), miniboss(10, 6))", [5m] },
        };
    }

    /// <summary>
    /// Тестовые данные для программ верхнего уровня.
    /// </summary>
    public static TheoryData<string, List<decimal>> GetProgramTests()
    {
        return new TheoryData<string, List<decimal>>
        {
            // Минимальная программа с выводом
            {
                """
                bello!
                tulalilloo ti amo (1) naidu!
                """,
                [1m]
            },

            // Константы и переменные
            {
                """
                bello!
                trusela pi Papaya 3.14 naidu!
                poop x Papaya naidu!
                x lumai 5 naidu!
                tulalilloo ti amo (x) naidu!
                """,
                [5m]
            },

            // Присваивание без вывода (тест синтаксиса)
            {
                """
                bello!
                poop x Papaya naidu!
                x lumai 5 naidu!
                """,
                []
            },

            // Константы belloPi и belloE
            {
                """
                bello!
                trusela pi Papaya belloPi naidu!
                trusela e Papaya belloE naidu!
                """,
                []
            },
        };
    }

    /// <summary>
    /// Тестовые данные для проверки ввода/вывода через FakeEnvironment.
    /// </summary>
    public static TheoryData<string, decimal[], decimal[]> GetIoExecutionTests()
    {
        return new TheoryData<string, decimal[], decimal[]>
        {
            {
                """
                bello!
                poop x Papaya naidu!
                guoleila (x) naidu!
                tulalilloo ti amo (x melomo 1) naidu!
                """,
                new[] { 5m },
                new[] { 6m }
            },
            {
                """
                bello!
                poop a Papaya naidu!
                poop b Papaya naidu!
                guoleila (a) naidu!
                guoleila (b) naidu!
                tulalilloo ti amo (a melomo b) naidu!
                """,
                new[] { 10m, 20m },
                new[] { 30m }
            },
            {
                """
                bello!
                poop x Papaya naidu!
                guoleila (x) naidu!
                x lumai x dibotada x naidu!
                tulalilloo ti amo (x) naidu!
                """,
                new[] { 5m },
                new[] { 25m }
            },
        };
    }

    /// <summary>
    /// Тестовые данные для проверки ошибок.
    /// </summary>
    public static TheoryData<string> GetErrorTests()
    {
        return new TheoryData<string>
        {
            "(2 melomo 3",
            "2 melomo 3)",
            ")2 melomo 3(",
            "2 melomo",
            "()",
            "muak(5",
            "muak 5)",
            "muak(,)",
            "10 poopaye 0",
            "10 pado 0",
            "x",
            "x melomo 5",
            "!Hello!",

            """
            trusela pi Papaya 3.14 naidu!
            """,
            """
            bello!
            poop x Papaya
            """,
            """
            bello!
            trusela pi Papaya x naidu!
            """,
            """
            bello!
            poop x Papaya
            x lumai 5 naidu!
            """,
            """
            bello!
            poop x Papaya naidu!
            x lumai 5
            """,
            """
            bello!
            poop x Papaya naidu!
            tulalilloo ti amo (x)
            """,
        };
    }
}
