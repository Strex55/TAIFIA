using Interpreter;

namespace Interpreter;

/// <summary>
/// Консольная программа-интерпретатор языка Minion#.
/// </summary>
internal static class Program
{
    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.Error.WriteLine("Использование: Interpreter <путь_к_файлу.mns>");
            Environment.Exit(1);
            return;
        }

        string filePath = args[0];
        if (!File.Exists(filePath))
        {
            Console.Error.WriteLine($"Ошибка: файл '{filePath}' не найден.");
            Environment.Exit(1);
            return;
        }

        try
        {
            string code = File.ReadAllText(filePath);
            Interpreter interpreter = new();
            interpreter.Execute(code);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Ошибка выполнения: {ex.Message}");
            Environment.Exit(1);
        }
    }
}