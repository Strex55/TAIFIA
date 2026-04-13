namespace Execution;

/// <summary>
/// Класс для работы со встроенными функциями языка Minion#.
/// </summary>
public static class BuiltinFunctions
{
    /// <summary>
    /// Вызывает встроенную функцию по имени.
    /// </summary>
    /// <param name="name">Имя функции.</param>
    /// <param name="arguments">Список аргументов.</param>
    /// <returns>Результат выполнения функции.</returns>
    public static decimal Invoke(string name, List<decimal> arguments)
    {
        return name switch
        {
            "muak" => Muak(arguments),
            "miniboss" => Miniboss(arguments),
            "bigboss" => Bigboss(arguments),
            _ => throw new ArgumentException($"Неизвестная функция: {name}", nameof(name))
        };
    }

    /// <summary>
    /// Вычисляет абсолютное значение числа (muak).
    /// </summary>
    private static decimal Muak(List<decimal> arguments)
    {
        if (arguments.Count != 1)
        {
            throw new ArgumentException("Функция 'muak' принимает 1 аргумент", nameof(arguments));
        }

        return Math.Abs(arguments[0]);
    }

    /// <summary>
    /// Вычисляет минимум из аргументов (miniboss).
    /// </summary>
    private static decimal Miniboss(List<decimal> arguments)
    {
        if (arguments.Count == 0)
        {
            throw new ArgumentException("Функция 'miniboss' требует хотя бы один аргумент", nameof(arguments));
        }

        return arguments.Min();
    }

    /// <summary>
    /// Вычисляет максимум из аргументов (bigboss).
    /// </summary>
    private static decimal Bigboss(List<decimal> arguments)
    {
        if (arguments.Count == 0)
        {
            throw new ArgumentException("Функция 'bigboss' требует хотя бы один аргумент", nameof(arguments));
        }

        return arguments.Max();
    }
}