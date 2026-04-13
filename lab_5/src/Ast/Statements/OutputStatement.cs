using Ast.Expressions;

namespace Ast.Statements;

/// <summary>
/// Узел для инструкции вывода (tulalilloo ti amo).
/// </summary>
public class OutputStatement : Statement
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="OutputStatement"/>.
    /// </summary>
    /// <param name="expression">Выражение для вывода.</param>
    public OutputStatement(Expression expression)
    {
        Expression = expression ?? throw new ArgumentNullException(nameof(expression));
    }

    /// <summary>
    /// Выражение для вывода.
    /// </summary>
    public Expression Expression { get; }

    /// <inheritdoc/>
    public override void Accept(IAstVisitor visitor)
    {
        visitor.VisitOutputStatement(this);
    }
}