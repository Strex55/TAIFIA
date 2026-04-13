namespace Ast.Expressions;

/// <summary>
/// Узел для унарного выражения.
/// </summary>
public class UnaryExpression : Expression
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UnaryExpression"/>.
    /// </summary>
    /// <param name="op">Оператор.</param>
    /// <param name="operand">Операнд.</param>
    public UnaryExpression(UnaryOperator op, Expression operand)
    {
        Operator = op;
        Operand = operand ?? throw new ArgumentNullException(nameof(operand));
    }

    /// <summary>
    /// Тип унарного оператора.
    /// </summary>
    public enum UnaryOperator
    {
        /// <summary>
        /// Унарный плюс (+).
        /// </summary>
        Plus,

        /// <summary>
        /// Унарный минус (-).
        /// </summary>
        Minus,

        /// <summary>
        /// Логическое НЕ.
        /// </summary>
        LogicalNot,
    }

    /// <summary>
    /// Оператор.
    /// </summary>
    public UnaryOperator Operator { get; }

    /// <summary>
    /// Операнд.
    /// </summary>
    public Expression Operand { get; }

    /// <inheritdoc/>
    public override void Accept(IAstVisitor visitor)
    {
        visitor.VisitUnaryExpression(this);
    }
}