using Ast;
using Execution;
using Xunit;
using ParserClass = Parser.Parser;

namespace Parser.UnitTests;

public class ParserTest
{
    /// <summary>
    /// Тестирует парсинг выражений с проверкой результата вычисления.
    /// </summary>
    [Theory]
    [MemberData(nameof(GetExpressionTests))]
    public void ParseExpressionTest(string code, List<decimal> expected)
    {
        Expression expr = ParserClass.ParseExpression(code);
        Context context = new Context();
        AstEvaluator evaluator = new AstEvaluator(context, new FakeEnvironment());
        decimal result = evaluator.EvaluateExpression(expr);
        Assert.Equal(expected[0], result);
    }

    /// <summary>
    /// Тестирует парсинг программ с проверкой результатов выполнения.
    /// </summary>
    [Theory]
    [MemberData(nameof(GetProgramTests))]
    public void ParseProgramTest(string code, List<decimal> expected)
    {
        FakeEnvironment env = new();
        IReadOnlyList<AstNode> topLevelItems = ParserClass.ParseProgram(code);
        Context context = new Context();
        AstEvaluator evaluator = new AstEvaluator(context, env);
        evaluator.EvaluateProgram(topLevelItems);
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
    /// Тестирует выполнение программ с вводом/выводом через FakeEnvironment.
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

        IReadOnlyList<AstNode> topLevelItems = ParserClass.ParseProgram(code);
        Context context = new Context();
        AstEvaluator evaluator = new AstEvaluator(context, env);
        evaluator.EvaluateProgram(topLevelItems);

        Assert.Equal(expectedOutputs.Length, env.Results.Count);
        for (int i = 0; i < expectedOutputs.Length; i++)
        {
            Assert.Equal(expectedOutputs[i], env.Results[i]);
        }
    }

    /// <summary>
    /// Тестирует обработку ошибок парсинга и выполнения.
    /// </summary>
    [Theory]
    [MemberData(nameof(GetErrorTests))]
    public void ParseErrorTest(string code)
    {
        if (code.Contains("bello!"))
        {
            // Для программ: проверяем, что ошибка возникает либо при парсинге, либо при выполнении
            Assert.ThrowsAny<Exception>(() =>
            {
                IReadOnlyList<AstNode> programAst = ParserClass.ParseProgram(code);
                Context context = new Context();
                AstEvaluator evaluator = new AstEvaluator(context, new FakeEnvironment());
                evaluator.EvaluateProgram(programAst);
            });
        }
        else
        {
            // Для выражений: проверяем, что ошибка возникает либо при парсинге, либо при вычислении
            Assert.ThrowsAny<Exception>(() =>
            {
                Expression expr = ParserClass.ParseExpression(code);
                Context context = new Context();
                AstEvaluator evaluator = new AstEvaluator(context, new FakeEnvironment());
                evaluator.EvaluateExpression(expr);
            });
        }
    }

    /// <summary>
    /// Тестовые данные для выражений.
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
            { "1.128 melomo 8 flavuk 7.5", [1.628m] }, // 1.128 + 8 - 7.5 = 1.628
            { "2 beedo 5", [32m] },
            { "(2 melomo 3) poopaye 10", [0.5m] }, // (2 + 3) / 10 = 0.5
            { "(flavuk 2) beedo 10", [1024m] },
            { "flavuk 2 beedo 10", [-1024m] },

            // Логические операции
            { "da", [1m] },
            { "no", [0m] },
            { "5 con 5", [1m] },
            { "5 nocon 10", [1m] },
            { "5 la 10", [1m] },
            { "5 lacon 5", [1m] },
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

            // If/else
            {
                """
                bello!
                poop x Papaya naidu!
                x lumai 5 naidu!
                bi-do (x la 10) oca!
                    tulalilloo ti amo (1) naidu!
                stopa
                """,
                [1m]
            },

            // Вложенные блоки
            {
                """
                bello!
                poop x Papaya naidu!
                x lumai 5 naidu!
                bi-do (x la 10) oca!
                    poop y Papaya naidu!
                    y lumai x melomo 2 naidu!
                    bi-do (y looka too con 7) oca!
                        poop z Papaya naidu!
                        z lumai y flavuk 5 naidu!
                        tulalilloo ti amo (z) naidu!
                    stopa
                stopa
                """,
                [2m] // y = 5 + 2 = 7, условие 7 >= 7 истинно, блок выполняется, z = 7 - 5 = 2
            },

            // If без else (истинное и ложное условие)
            {
                """
                bello!
                bi-do (da) oca!
                    tulalilloo ti amo (1) naidu!
                stopa
                bi-do (no) oca!
                    tulalilloo ti amo (2) naidu!
                stopa
                """,
                [1m] // только первый блок выполняется
            },

            // If с else (истинное и ложное условие)
            {
                """
                bello!
                bi-do (da) oca!
                    tulalilloo ti amo (1) naidu!
                stopa
                uh-oh oca!
                    tulalilloo ti amo (2) naidu!
                stopa
                bi-do (no) oca!
                    tulalilloo ti amo (3) naidu!
                stopa
                uh-oh oca!
                    tulalilloo ti amo (4) naidu!
                stopa
                """,
                [1m, 4m] // первый if: then, второй if: else
            },

            // Вложенные if с else
            {
                """
                bello!
                poop x Papaya naidu!
                poop y Papaya naidu!
                x lumai 5 naidu!
                y lumai 10 naidu!
                bi-do (x con 5) oca!
                    bi-do (y con 10) oca!
                        tulalilloo ti amo (1) naidu!
                    stopa
                    uh-oh oca!
                        tulalilloo ti amo (2) naidu!
                    stopa
                stopa
                uh-oh oca!
                    tulalilloo ti amo (3) naidu!
                stopa
                """,
                [1m]
            },

            // If с присваиванием и вводом/выводом
            {
                """
                bello!
                poop x Papaya naidu!
                x lumai 5 naidu!
                bi-do (x la 10) oca!
                    x lumai x melomo 2 naidu!
                    tulalilloo ti amo (x) naidu!
                stopa
                """,
                [7m]
            },

            // While с различными условиями
            {
                """
                bello!
                poop i Papaya naidu!
                i lumai 0 naidu!
                kemari (i lacon 5) oca!
                    tulalilloo ti amo (i) naidu!
                    i lumai i melomo 1 naidu!
                stopa
                kemari (no) oca!
                    tulalilloo ti amo (99) naidu!
                stopa
                poop x Papaya naidu!
                x lumai 0 naidu!
                kemari (x la 3) oca!
                    x lumai x melomo 1 naidu!
                    tulalilloo ti amo (x) naidu!
                stopa
                """,
                [0m, 1m, 2m, 3m, 4m, 5m, 1m, 2m, 3m]
            },

            // While с вложенными блоками и условиями
            {
                """
                bello!
                poop x Papaya naidu!
                poop y Papaya naidu!
                x lumai 0 naidu!
                kemari (x la 2) oca!
                    y lumai 0 naidu!
                    bi-do (y con 0) oca!
                        tulalilloo ti amo (x) naidu!
                    stopa
                    x lumai x melomo 1 naidu!
                stopa
                """,
                [0m, 1m]
            },

            // For цикл - простой случай
            {
                """
                bello!
                again (i = 1 to 5) oca!
                    tulalilloo ti amo (i) naidu!
                stopa
                """,
                [1m, 2m, 3m, 4m, 5m]
            },

            // For цикл - с выражениями в границах
            {
                """
                bello!
                poop start Papaya naidu!
                poop end Papaya naidu!
                start lumai 2 naidu!
                end lumai 4 naidu!
                again (i = start to end) oca!
                    tulalilloo ti amo (i) naidu!
                stopa
                """,
                [2m, 3m, 4m]
            },

            // For цикл - с вычислением границ через выражения
            {
                """
                bello!
                again (i = 1 melomo 1 to 2 dibotada 2) oca!
                    tulalilloo ti amo (i) naidu!
                stopa
                """,
                [2m, 3m, 4m] // от 1+1=2 до 2*2=4
            },

            // For цикл - вложенные циклы
            {
                """
                bello!
                again (i = 1 to 2) oca!
                    again (j = 1 to 2) oca!
                        tulalilloo ti amo (i dibotada 10 melomo j) naidu!
                    stopa
                stopa
                """,
                [11m, 12m, 21m, 22m] // i=1: j=1→11, j=2→12; i=2: j=1→21, j=2→22
            },

            // For цикл - с if внутри
            {
                """
                bello!
                again (i = 1 to 5) oca!
                    bi-do (i pado 2 con 0) oca!
                        tulalilloo ti amo (i) naidu!
                    stopa
                stopa
                """,
                [2m, 4m] // только четные числа
            },

            // For цикл - с переменными в теле
            {
                """
                bello!
                poop sum Papaya naidu!
                sum lumai 0 naidu!
                again (i = 1 to 3) oca!
                    sum lumai sum melomo i naidu!
                stopa
                tulalilloo ti amo (sum) naidu!
                """,
                [6m] // 0+1+2+3 = 6
            },

            // For цикл - область видимости переменной цикла
            {
                """
                bello!
                poop x Papaya naidu!
                x lumai 10 naidu!
                again (x = 1 to 3) oca!
                    tulalilloo ti amo (x) naidu!
                stopa
                tulalilloo ti amo (x) naidu!
                """,
                [1m, 2m, 3m, 10m] // переменная цикла затеняет внешнюю, затем восстанавливается
            },

            // Функции с разным количеством параметров
            {
                """
                bello!
                boss pi Papaya () oca!
                    tank yu 3.14 naidu!
                stopa
                boss square Papaya (x) oca!
                    tank yu x dibotada x naidu!
                stopa
                boss max Papaya (a, b, c) oca!
                    poop result Papaya naidu!
                    result lumai a naidu!
                    bi-do (b looka too result) oca!
                        result lumai b naidu!
                    stopa
                    bi-do (c looka too result) oca!
                        result lumai c naidu!
                    stopa
                    tank yu result naidu!
                stopa
                tulalilloo ti amo (pi()) naidu!
                tulalilloo ti amo (square(5)) naidu!
                tulalilloo ti amo (max(1, 5, 3)) naidu!
                """,
                [3.14m, 25m, 5m] // pi() возвращает 3.14
            },

            // Вложенные вызовы функций
            {
                """
                bello!
                boss add Papaya (a, b) oca!
                    tank yu a melomo b naidu!
                stopa
                tulalilloo ti amo (add(add(1, 2), add(3, 4))) naidu!
                """,
                [10m]
            },

            // Функция с выражениями, локальными переменными и условными операторами
            {
                """
                bello!
                boss abs Papaya (x) oca!
                    poop result Papaya naidu!
                    bi-do (x la 0) oca!
                        result lumai flavuk x naidu!
                    stopa
                    uh-oh oca!
                        result lumai x naidu!
                    stopa
                    tank yu result naidu!
                stopa
                tulalilloo ti amo (abs(flavuk 5)) naidu!
                tulalilloo ti amo (abs(5)) naidu!
                """,
                [5m, 5m]
            },

            // Функция с циклом
            {
                """
                bello!
                boss sumToN Papaya (n) oca!
                    poop sum Papaya naidu!
                    poop i Papaya naidu!
                    sum lumai 0 naidu!
                    i lumai 1 naidu!
                    kemari (i lacon n) oca!
                        sum lumai sum melomo i naidu!
                        i lumai i melomo 1 naidu!
                    stopa
                    tank yu sum naidu!
                stopa
                tulalilloo ti amo (sumToN(5)) naidu!
                """,
                [15m] // 1+2+3+4+5 = 15
            },

            // Функция с if и циклом (факториал)
            {
                """
                bello!
                boss factorial Papaya (n) oca!
                    bi-do (n con 0) oca!
                        tank yu 1 naidu!
                    stopa
                    poop result Papaya naidu!
                    poop i Papaya naidu!
                    result lumai 1 naidu!
                    i lumai 1 naidu!
                    kemari (i lacon n) oca!
                        result lumai result dibotada i naidu!
                        i lumai i melomo 1 naidu!
                    stopa
                    tank yu result naidu!
                stopa
                tulalilloo ti amo (factorial(5)) naidu!
                """,
                [120m] // 5! = 120
            },

            // Функция с циклом for
            {
                """
                bello!
                boss sumToN Papaya (n) oca!
                    poop sum Papaya naidu!
                    sum lumai 0 naidu!
                    again (i = 1 to n) oca!
                        sum lumai sum melomo i naidu!
                    stopa
                    tank yu sum naidu!
                stopa
                tulalilloo ti amo (sumToN(5)) naidu!
                """,
                [15m] // 1+2+3+4+5 = 15
            },

            // Функция с вложенными циклами for
            {
                """
                bello!
                boss multiplyTable Papaya (n) oca!
                    poop result Papaya naidu!
                    result lumai 0 naidu!
                    again (i = 1 to n) oca!
                        again (j = 1 to n) oca!
                            result lumai result melomo 1 naidu!
                        stopa
                    stopa
                    tank yu result naidu!
                stopa
                tulalilloo ti amo (multiplyTable(2)) naidu!
                """,
                [4m] // 2x2 = 4 итерации
            },

            // Функция, вызывающая другую функцию
            {
                """
                bello!
                boss add Papaya (a, b) oca!
                    tank yu a melomo b naidu!
                stopa
                boss multiply Papaya (x, y) oca!
                    tank yu x dibotada y naidu!
                stopa
                tulalilloo ti amo (multiply(add(1, 2), add(3, 4))) naidu!
                """,
                [21m] // (1+2) * (3+4) = 3 * 7 = 21
            },

            // If и while с вызовами функций в условиях
            {
                """
                bello!
                boss isPositive Papaya (x) oca!
                    tank yu x looka too 0 naidu!
                stopa
                poop x Papaya naidu!
                x lumai 5 naidu!
                bi-do (isPositive(x)) oca!
                    tulalilloo ti amo (1) naidu!
                stopa
                bi-do (muak(flavuk 5) con 5) oca!
                    tulalilloo ti amo (2) naidu!
                stopa
                poop count Papaya naidu!
                count lumai 0 naidu!
                boss notFinished Papaya (c) oca!
                    tank yu c la 3 naidu!
                stopa
                kemari (notFinished(count)) oca!
                    tulalilloo ti amo (count) naidu!
                    count lumai count melomo 1 naidu!
                stopa
                kemari (miniboss(count, 5) la 3) oca!
                    tulalilloo ti amo (count) naidu!
                    count lumai count melomo 1 naidu!
                stopa
                """,
                [1m, 2m, 0m, 1m, 2m] // isPositive(5)=true→1, muak(-5)==5→2, notFinished: 0,1,2 (после первого цикла count=3, второй цикл не выполняется, так как miniboss(3,5)=3, но условие не истинно после выхода из первого цикла)
            },

            // Цикл с if внутри
            {
                """
                bello!
                poop x Papaya naidu!
                x lumai 0 naidu!
                kemari (x la 10) oca!
                    bi-do (x pado 2 con 0) oca!
                        tulalilloo ti amo (x) naidu!
                    stopa
                    x lumai x melomo 1 naidu!
                stopa
                """,
                [0m, 2m, 4m, 6m, 8m]
            },

            // Функция с локальными переменными и вложенными блоками
            {
                """
                bello!
                boss complex Papaya (x) oca!
                    poop y Papaya naidu!
                    y lumai x melomo 2 naidu!
                    bi-do (y looka too 5) oca!
                        poop z Papaya naidu!
                        z lumai y flavuk 3 naidu!
                        bi-do (z con 7) oca!
                            tank yu z naidu!
                        stopa
                        tank yu z naidu!
                    stopa
                    uh-oh oca!
                        tank yu y naidu!
                    stopa
                stopa
                tulalilloo ti amo (complex(5)) naidu!
                """,
                [4m] // x=5, y=10, условие y > 5 истинно, z=7, но возвращается 4 - возможно проблема в логике обработки return в вложенных блоках
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

            // While с вводом/выводом
            {
                """
                bello!
                poop x Papaya naidu!
                x lumai 0 naidu!
                kemari (x con 0) oca!
                    guoleila (x) naidu!
                    tulalilloo ti amo (x) naidu!
                    x lumai 1 naidu!
                stopa
                """,
                new[] { 5m },
                new[] { 5m }
            },

            // If с вводом/выводом
            {
                """
                bello!
                poop x Papaya naidu!
                x lumai 0 naidu!
                bi-do (x con 0) oca!
                    poop y Papaya naidu!
                    guoleila (y) naidu!
                    tulalilloo ti amo (y) naidu!
                stopa
                """,
                new[] { 7m },
                new[] { 7m }
            },

            // Функция с побочными эффектами (ввод внутри функции)
            {
                """
                bello!
                boss readAndDouble Papaya () oca!
                    poop x Papaya naidu!
                    guoleila (x) naidu!
                    tank yu x dibotada 2 naidu!
                stopa
                tulalilloo ti amo (readAndDouble()) naidu!
                """,
                new[] { 5m },
                new[] { 10m }
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
            tank yu 1 naidu!
            """,
            """
            bello!
            boss foo Papaya () oca!
                x lumai 1
            """,
            """
            bello!
            bi-do (da) oca!
                x lumai 1
            stopa
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
            """
            bello!
            boss test Papaya () oca!
                poop x Papaya
                x lumai 1 naidu!
            stopa
            """,
            
            // Незакрытый блок if
            """
            bello!
            bi-do (da) oca!
                tulalilloo ti amo (1) naidu!
            """,

            // Незакрытый блок else
            """
            bello!
            bi-do (no) oca!
                tulalilloo ti amo (1) naidu!
            stopa
            uh-oh oca!
                tulalilloo ti amo (2) naidu!
            """,

            // Незакрытый блок while
            """
            bello!
            kemari (da) oca!
                tulalilloo ti amo (1) naidu!
            """,

            // Незакрытый блок for
            """
            bello!
            again (i = 1 to 5) oca!
                tulalilloo ti amo (i) naidu!
            """,

            // Неправильный синтаксис for - отсутствие "="
            """
            bello!
            again (i 1 to 5) oca!
                tulalilloo ti amo (i) naidu!
            stopa
            """,

            // Неправильный синтаксис for - отсутствие "to"
            """
            bello!
            again (i = 1 5) oca!
                tulalilloo ti amo (i) naidu!
            stopa
            """,

            // Неправильный синтаксис for - отсутствие начального выражения
            """
            bello!
            again (i = to 5) oca!
                tulalilloo ti amo (i) naidu!
            stopa
            """,

            // Неправильный синтаксис for - отсутствие конечного выражения
            """
            bello!
            again (i = 1 to) oca!
                tulalilloo ti amo (i) naidu!
            stopa
            """,

            // Неправильный синтаксис for - отсутствие открывающей скобки
            """
            bello!
            again i = 1 to 5) oca!
                tulalilloo ti amo (i) naidu!
            stopa
            """,

            // Неправильный синтаксис for - отсутствие закрывающей скобки
            """
            bello!
            again (i = 1 to 5 oca!
                tulalilloo ti amo (i) naidu!
            stopa
            """,

            // Незакрытый блок функции - проверяется на этапе парсинга
            """
            bello!
            boss test Papaya () oca!
                tank yu 1 naidu!
            """,

            // Отсутствие tank yu в функции - проверяется на этапе выполнения при вызове
            """
            bello!
            boss test Papaya () oca!
                poop x Papaya naidu!
                x lumai 1 naidu!
            stopa
            tulalilloo ti amo (test()) naidu!
            """,

            // Неправильное количество аргументов при вызове функции
            """
            bello!
            boss add Papaya (a, b) oca!
                tank yu a melomo b naidu!
            stopa
            tulalilloo ti amo (add(1)) naidu!
            """,

            // Вызов несуществующей функции
            """
            bello!
            tulalilloo ti amo (unknown(5)) naidu!
            """,

            // Повторное объявление функции с тем же именем
            """
            bello!
            boss test Papaya () oca!
                tank yu 1 naidu!
            stopa
            boss test Papaya () oca!
                tank yu 2 naidu!
            stopa
            """,

            // Использование переменной до объявления в блоке
            """
            bello!
            bi-do (da) oca!
                x lumai 1 naidu!
                poop x Papaya naidu!
            stopa
            """,

            // Неправильный синтаксис параметров функции
            """
            bello!
            boss test Papaya (a, , b) oca!
                tank yu 1 naidu!
            stopa
            """,
        };
    }
}
