using Ast.Expressions;

namespace Ast.Statements;

/// <summary>
/// Узел для цикла for (again).
/// </summary>
public class ForStatement : Statement
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ForStatement"/>.
    /// </summary>
    /// <param name="variableName">Имя переменной цикла.</param>
    /// <param name="startExpression">Начальное значение (выражение).</param>
    /// <param name="endExpression">Конечное значение (выражение).</param>
    /// <param name="body">Тело цикла.</param>
    public ForStatement(string variableName, Expression startExpression, Expression endExpression, Block body)
    {
        VariableName = variableName ?? throw new ArgumentNullException(nameof(variableName));
        StartExpression = startExpression ?? throw new ArgumentNullException(nameof(startExpression));
        EndExpression = endExpression ?? throw new ArgumentNullException(nameof(endExpression));
        Body = body ?? throw new ArgumentNullException(nameof(body));
    }

    /// <summary>
    /// Имя переменной цикла.
    /// </summary>
    public string VariableName { get; }

    /// <summary>
    /// Начальное значение переменной.
    /// </summary>
    public Expression StartExpression { get; }

    /// <summary>
    /// Конечное значение переменной.
    /// </summary>
    public Expression EndExpression { get; }

    /// <summary>
    /// Тело цикла.
    /// </summary>
    public Block Body { get; }

    /// <inheritdoc/>
    public override void Accept(IAstVisitor visitor)
    {
        visitor.VisitForStatement(this);
    }
}
