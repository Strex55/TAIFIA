namespace Ast.Expressions;

/// <summary>
/// Узел для бинарного выражения.
/// </summary>
public class BinaryExpression : Expression
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="BinaryExpression"/>.
    /// </summary>
    /// <param name="left">Левый операнд.</param>
    /// <param name="op">Оператор.</param>
    /// <param name="right">Правый операнд.</param>
    public BinaryExpression(Expression left, BinaryOperator op, Expression right)
    {
        Left = left ?? throw new ArgumentNullException(nameof(left));
        Operator = op;
        Right = right ?? throw new ArgumentNullException(nameof(right));
    }

    /// <summary>
    /// Тип бинарного оператора.
    /// </summary>
    public enum BinaryOperator
    {
        /// <summary>
        /// Сложение (+).
        /// </summary>
        Add,

        /// <summary>
        /// Вычитание (-).
        /// </summary>
        Subtract,

        /// <summary>
        /// Умножение (*).
        /// </summary>
        Multiply,

        /// <summary>
        /// Деление (/).
        /// </summary>
        Divide,

        /// <summary>
        /// Остаток от деления (%).
        /// </summary>
        Modulo,

        /// <summary>
        /// Возведение в степень (**).
        /// </summary>
        Power,

        /// <summary>
        /// Равенство (==).
        /// </summary>
        Equal,

        /// <summary>
        /// Неравенство (!=).
        /// </summary>
        NotEqual,

        /// <summary>
        /// Меньше (&lt;).
        /// </summary>
        LessThan,

        /// <summary>
        /// Меньше или равно (&lt;=).
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// Больше (&gt;).
        /// </summary>
        GreaterThan,

        /// <summary>
        /// Больше или равно (&gt;=).
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// Логическое И.
        /// </summary>
        LogicalAnd,

        /// <summary>
        /// Логическое ИЛИ.
        /// </summary>
        LogicalOr,
    }

    /// <summary>
    /// Левый операнд.
    /// </summary>
    public Expression Left { get; }

    /// <summary>
    /// Оператор.
    /// </summary>
    public BinaryOperator Operator { get; }

    /// <summary>
    /// Правый операнд.
    /// </summary>
    public Expression Right { get; }

    /// <inheritdoc/>
    public override void Accept(IAstVisitor visitor)
    {
        visitor.VisitBinaryExpression(this);
    }
}