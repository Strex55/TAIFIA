namespace Ast.Expressions;

/// <summary>
/// Узел для числового литерала.
/// </summary>
public class NumberLiteral : Expression
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="NumberLiteral"/>.
    /// </summary>
    /// <param name="value">Значение числа.</param>
    public NumberLiteral(decimal value)
    {
        Value = value;
    }

    /// <summary>
    /// Значение числа.
    /// </summary>
    public decimal Value { get; }

    /// <inheritdoc/>
    public override void Accept(IAstVisitor visitor)
    {
        visitor.VisitNumberLiteral(this);
    }
}