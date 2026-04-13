using Ast.Expressions;

namespace Ast.Statements;

/// <summary>
/// Узел для цикла while (kemari).
/// </summary>
public class WhileStatement : Statement
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="WhileStatement"/>.
    /// </summary>
    /// <param name="condition">Условие цикла.</param>
    /// <param name="body">Тело цикла.</param>
    public WhileStatement(Expression condition, Block body)
    {
        Condition = condition ?? throw new ArgumentNullException(nameof(condition));
        Body = body ?? throw new ArgumentNullException(nameof(body));
    }

    /// <summary>
    /// Условие цикла.
    /// </summary>
    public Expression Condition { get; }

    /// <summary>
    /// Тело цикла.
    /// </summary>
    public Block Body { get; }

    /// <inheritdoc/>
    public override void Accept(IAstVisitor visitor)
    {
        visitor.VisitWhileStatement(this);
    }
}