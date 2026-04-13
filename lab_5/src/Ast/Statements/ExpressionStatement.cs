using Ast.Expressions;

namespace Ast.Statements;

/// <summary>
/// Узел для выражения как инструкции.
/// </summary>
public class ExpressionStatement : Statement
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ExpressionStatement"/>.
    /// </summary>
    /// <param name="expression">Выражение.</param>
    public ExpressionStatement(Expression expression)
    {
        Expression = expression ?? throw new ArgumentNullException(nameof(expression));
    }

    /// <summary>
    /// Выражение.
    /// </summary>
    public Expression Expression { get; }

    /// <inheritdoc/>
    public override void Accept(IAstVisitor visitor)
    {
        visitor.VisitExpressionStatement(this);
    }
}