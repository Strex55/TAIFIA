namespace Parser;

/// <summary>
/// Класс для работы со встроенными функциями языка Minion#.
/// </summary>
public static class BuiltinFunctions
{
    /// <summary>
    /// Вызывает встроенную функцию по имени с заданными аргументами.
    /// </summary>
    /// <param name="name">Имя функции (muak, miniboss, bigboss)</param>
    /// <param name="arguments">Список аргументов функции</param>
    /// <returns>Результат выполнения функции</returns>
    /// <exception cref="ArgumentException">Если имя функции неизвестно</exception>
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
    /// Вычисляет модуль числа (абсолютное значение).
    /// </summary>
    private static decimal Muak(List<decimal> arguments)
    {
        if (arguments.Count != 1)
        {
            throw new ArgumentException("Функция muak принимает ровно 1 аргумент", nameof(arguments));
        }

        return Math.Abs(arguments[0]);
    }

    /// <summary>
    /// Находит минимальное значение среди аргументов.
    /// </summary>
    private static decimal Miniboss(List<decimal> arguments)
    {
        if (arguments.Count == 0)
        {
            throw new ArgumentException("Функция miniboss требует хотя бы один аргумент", nameof(arguments));
        }

        return arguments.Min();
    }

    /// <summary>
    /// Находит максимальное значение среди аргументов.
    /// </summary>
    private static decimal Bigboss(List<decimal> arguments)
    {
        if (arguments.Count == 0)
        {
            throw new ArgumentException("Функция bigboss требует хотя бы один аргумент", nameof(arguments));
        }

        return arguments.Max();
    }
}