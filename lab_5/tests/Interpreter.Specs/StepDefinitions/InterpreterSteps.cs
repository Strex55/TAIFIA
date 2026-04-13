#pragma warning disable SA1309
using Execution;

using Interpreter;

using Reqnroll;

namespace Interpreter.Specs.StepDefinitions;

/// <summary>
/// Шаги для тестирования интерпретатора через Reqnroll.
/// </summary>
[Binding]
public class InterpreterSteps
{
    private string? _programCode;
    private string? _actualOutput;
    private static readonly object ConsoleLock = new();

    /// <summary>
    /// Сохраняет код программы для выполнения.
    /// </summary>
    /// <param name="multilineText">Исходный код программы.</param>
    [Given(@"я написал программу:")]
    public void GivenIHaveWrittenProgram(string multilineText)
    {
        _programCode = multilineText;
    }

    /// <summary>
    /// Выполняет программу с заданным вводом и сохраняет результат.
    /// </summary>
    /// <param name="multilineText">Входные данные для программы.</param>
    [When(@"я ввожу в консоли:")]
    public void WhenIEnterInConsole(string multilineText)
    {
        if (_programCode == null)
        {
            throw new InvalidOperationException("Программный код необходимо задать перед выполнением");
        }

        _actualOutput = ExecuteProgram(_programCode, multilineText);
    }

    /// <summary>
    /// Проверяет, что фактический вывод соответствует ожидаемому.
    /// </summary>
    /// <param name="multilineText">Ожидаемый вывод программы.</param>
    [Then(@"я увижу в консоли:")]
    public void ThenIShouldSeeInConsole(string multilineText)
    {
        Assert.Equal(multilineText, _actualOutput ?? string.Empty);
    }

    private static string ExecuteProgram(string programCode, string inputText)
    {
        using StringReader inputReader = new(inputText);
        using StringWriter outputWriter = new();

        TextReader originalIn = Console.In;
        TextWriter originalOut = Console.Out;

        lock (ConsoleLock)
        {
            try
            {
                Console.SetIn(inputReader);
                Console.SetOut(outputWriter);

                Interpreter interpreter = new(new ConsoleEnvironment());
                interpreter.Execute(programCode);
            }
            finally
            {
                Console.SetIn(originalIn);
                Console.SetOut(originalOut);
            }
        }

        return outputWriter.ToString().TrimEnd('\r', '\n');
    }
}
