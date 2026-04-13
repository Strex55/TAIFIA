#pragma warning disable SA1309 // Field names should not begin with underscore
using Execution;
using Parser;

namespace Interpreter;

/// <summary>
/// Интерпретатор языка Minion#.
/// Фасад для выполнения программ с управлением контекстом и окружением.
/// </summary>
public class Interpreter
{
    private readonly Context _context;
    private readonly IEnvironment _environment;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Interpreter"/>.
    /// </summary>
    /// <param name="environment">Окружение для ввода/вывода. Если не указано, используется ConsoleEnvironment.</param>
    public Interpreter(IEnvironment? environment = null)
    {
        _context = new Context();
        _environment = environment ?? new ConsoleEnvironment();
    }

    /// <summary>
    /// Выполняет программу на языке Minion#.
    /// </summary>
    /// <param name="code">Исходный код программы.</param>
    /// <exception cref="ArgumentNullException">Если code равен null.</exception>
    /// <exception cref="InvalidOperationException">При ошибках парсинга или выполнения.</exception>
    public void Execute(string code)
    {
        if (code == null)
        {
            throw new ArgumentNullException(nameof(code));
        }

        Parser.Parser.ParseProgram(code, _context, _environment);
    }
}