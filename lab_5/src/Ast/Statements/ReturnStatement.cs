using Ast.Expressions;

namespace Ast.Statements;

/// <summary>
/// Узел для инструкции возврата (tank yu).
/// </summary>
public class ReturnStatement : Statement
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ReturnStatement"/>.
    /// </summary>
    /// <param name="expression">Выражение для возврата.</param>
    public ReturnStatement(Expression expression)
    {
        Expression = expression ?? throw new ArgumentNullException(nameof(expression));
    }

    /// <summary>
    /// Выражение для возврата.
    /// </summary>
    public Expression Expression { get; }

    /// <inheritdoc/>
    public override void Accept(IAstVisitor visitor)
    {
        visitor.VisitReturnStatement(this);
    }
}