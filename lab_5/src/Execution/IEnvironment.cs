namespace Execution;

/// <summary>
/// Окружение для выполнения программы: ввод и вывод чисел.
/// </summary>
public interface IEnvironment
{
    /// <summary>
    /// Записывает число (для tulalilloo ti amo).
    /// </summary>
    void WriteNumber(decimal result);

    /// <summary>
    /// Читает число (для guoleila).
    /// </summary>
    decimal ReadNumber();
}