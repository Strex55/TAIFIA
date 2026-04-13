namespace Ast.Statements;

/// <summary>
/// Узел для блока кода (oca! ... stopa).
/// </summary>
public class Block : Statement
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Block"/>.
    /// </summary>
    /// <param name="statements">Список инструкций в блоке.</param>
    public Block(IReadOnlyList<Statement> statements)
    {
        Statements = statements ?? throw new ArgumentNullException(nameof(statements));
    }

    /// <summary>
    /// Список инструкций в блоке.
    /// </summary>
    public IReadOnlyList<Statement> Statements { get; }

    /// <inheritdoc/>
    public override void Accept(IAstVisitor visitor)
    {
        visitor.VisitBlock(this);
    }
}