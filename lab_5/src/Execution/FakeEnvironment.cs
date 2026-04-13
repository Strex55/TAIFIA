namespace Execution;

/// <summary>
/// Поддельное окружение: работает как настоящее, но не совершает реального ввода/вывода.
/// </summary>
public class FakeEnvironment : IEnvironment
{
    private readonly List<decimal> results = [];
    private readonly Queue<decimal> inputs = new();

    /// <summary>
    /// Результаты вывода (для tulalilloo ti amo).
    /// </summary>
    public IReadOnlyList<decimal> Results => results;

    /// <summary>
    /// Добавляет входное значение для чтения (для guoleila).
    /// </summary>
    public void AddInput(decimal value)
    {
        inputs.Enqueue(value);
    }

    public void WriteNumber(decimal result)
    {
        results.Add(result);
    }

    public decimal ReadNumber()
    {
        if (inputs.Count == 0)
        {
            throw new InvalidOperationException("Нет входных значений для чтения");
        }

        return inputs.Dequeue();
    }
}