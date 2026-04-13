namespace Ast.Statements;

/// <summary>
/// Узел для объявления переменной (poop).
/// </summary>
public class VariableDeclaration : Statement
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="VariableDeclaration"/>.
    /// </summary>
    /// <param name="name">Имя переменной.</param>
    public VariableDeclaration(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    /// <summary>
    /// Имя переменной.
    /// </summary>
    public string Name { get; }

    /// <inheritdoc/>
    public override void Accept(IAstVisitor visitor)
    {
        visitor.VisitVariableDeclaration(this);
    }
}