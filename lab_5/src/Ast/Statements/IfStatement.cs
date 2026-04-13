using Ast.Expressions;

namespace Ast.Statements;

/// <summary>
/// Узел для условной инструкции (bi-do).
/// </summary>
public class IfStatement : Statement
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="IfStatement"/>.
    /// </summary>
    /// <param name="condition">Условие.</param>
    /// <param name="thenBlock">Блок кода для истинного условия.</param>
    /// <param name="elseBlock">Блок кода для ложного условия (опционально).</param>
    public IfStatement(Expression condition, Block thenBlock, Block? elseBlock = null)
    {
        Condition = condition ?? throw new ArgumentNullException(nameof(condition));
        ThenBlock = thenBlock ?? throw new ArgumentNullException(nameof(thenBlock));
        ElseBlock = elseBlock;
    }

    /// <summary>
    /// Условие.
    /// </summary>
    public Expression Condition { get; }

    /// <summary>
    /// Блок кода для истинного условия.
    /// </summary>
    public Block ThenBlock { get; }

    /// <summary>
    /// Блок кода для ложного условия (else), может быть null.
    /// </summary>
    public Block? ElseBlock { get; }

    /// <inheritdoc/>
    public override void Accept(IAstVisitor visitor)
    {
        visitor.VisitIfStatement(this);
    }
}