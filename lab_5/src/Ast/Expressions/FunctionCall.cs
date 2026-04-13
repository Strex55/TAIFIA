namespace Ast.Expressions;

/// <summary>
/// Узел для вызова функции.
/// </summary>
public class FunctionCall : Expression
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="FunctionCall"/>.
    /// </summary>
    /// <param name="functionName">Имя функции.</param>
    /// <param name="arguments">Список аргументов.</param>
    public FunctionCall(string functionName, IReadOnlyList<Expression> arguments)
    {
        FunctionName = functionName ?? throw new ArgumentNullException(nameof(functionName));
        Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
    }

    /// <summary>
    /// Имя функции.
    /// </summary>
    public string FunctionName { get; }

    /// <summary>
    /// Список аргументов.
    /// </summary>
    public IReadOnlyList<Expression> Arguments { get; }

    /// <inheritdoc/>
    public override void Accept(IAstVisitor visitor)
    {
        visitor.VisitFunctionCall(this);
    }
}