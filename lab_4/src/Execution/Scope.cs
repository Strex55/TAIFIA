namespace Execution;

/// <summary>
/// Область видимости переменных.
/// </summary>
public class Scope
{
    private readonly Dictionary<string, decimal> variables = [];

    /// <summary>
    /// Читает переменную из этой области видимости. Возвращает false, если переменная не объявлена в этой области видимости.
    /// </summary>
    public bool TryGetVariable(string name, out decimal value)
    {
        if (variables.TryGetValue(name, out decimal v))
        {
            value = v;
            return true;
        }

        value = 0.0m;
        return false;
    }

    /// <summary>
    /// Присваивает переменную в этой области видимости. Возвращает false, если переменная не объявлена в этой области видимости.
    /// </summary>
    public bool TryAssignVariable(string name, decimal value)
    {
        if (variables.ContainsKey(name))
        {
            variables[name] = value;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Объявляет переменную в этой области видимости. Возвращает false, если переменная уже объявлена в этой области видимости.
    /// </summary>
    public bool TryDefineVariable(string name, decimal value)
    {
        if (variables.ContainsKey(name))
        {
            return false;
        }

        variables[name] = value;
        return true;
    }
}
