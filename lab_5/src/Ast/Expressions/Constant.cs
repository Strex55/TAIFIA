namespace Ast.Expressions;

/// <summary>
/// Узел для константы (belloPi, belloE).
/// </summary>
public class Constant : Expression
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Constant"/>.
    /// </summary>
    /// <param name="type">Тип константы.</param>
    public Constant(ConstantType type)
    {
        Type = type;
    }

    /// <summary>
    /// Тип константы.
    /// </summary>
    public enum ConstantType
    {
        /// <summary>
        /// Константа π (пи).
        /// </summary>
        Pi,

        /// <summary>
        /// Константа e (число Эйлера).
        /// </summary>
        E,
    }

    /// <summary>
    /// Тип константы.
    /// </summary>
    public ConstantType Type { get; }

    /// <inheritdoc/>
    public override void Accept(IAstVisitor visitor)
    {
        visitor.VisitConstant(this);
    }
}