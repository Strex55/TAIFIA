namespace Execution;

/// <summary>
/// Контекст выполнения: управляет областями видимости переменных.
/// </summary>
public class Context
{
    private readonly Stack<Scope> scopes = new();

    /// <summary>
    /// Создает новый контекст с глобальной областью видимости.
    /// </summary>
    public Context()
    {
        PushScope();
    }

    /// <summary>
    /// Добавляет новую область видимости в стек.
    /// </summary>
    public void PushScope()
    {
        scopes.Push(new Scope());
    }

    /// <summary>
    /// Удаляет текущую область видимости из стека.
    /// </summary>
    public void PopScope()
    {
        if (scopes.Count <= 1)
        {
            throw new InvalidOperationException("Нельзя удалить глобальную область видимости");
        }

        scopes.Pop();
    }

    /// <summary>
    /// Читает переменную из текущих областей видимости (от текущей к глобальной).
    /// </summary>
    public bool TryGetVariable(string name, out decimal value)
    {
        foreach (Scope scope in scopes)
        {
            if (scope.TryGetVariable(name, out value))
            {
                return true;
            }
        }

        value = 0.0m;
        return false;
    }

    /// <summary>
    /// Присваивает переменную в текущих областях видимости (от текущей к глобальной).
    /// </summary>
    public bool TryAssignVariable(string name, decimal value)
    {
        foreach (Scope scope in scopes)
        {
            if (scope.TryAssignVariable(name, value))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Объявляет переменную в текущей области видимости.
    /// </summary>
    public bool TryDefineVariable(string name, decimal value)
    {
        if (scopes.Count == 0)
        {
            return false;
        }

        return scopes.Peek().TryDefineVariable(name, value);
    }
}