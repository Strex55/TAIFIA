using Ast.Expressions;

namespace Ast.Statements;

/// <summary>
/// Узел для инструкции присваивания.
/// </summary>
public class AssignmentStatement : Statement
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AssignmentStatement"/>.
    /// </summary>
    /// <param name="variableName">Имя переменной.</param>
    /// <param name="expression">Выражение для присваивания.</param>
    public AssignmentStatement(string variableName, Expression expression)
    {
        VariableName = variableName ?? throw new ArgumentNullException(nameof(variableName));
        Expression = expression ?? throw new ArgumentNullException(nameof(expression));
    }

    /// <summary>
    /// Имя переменной.
    /// </summary>
    public string VariableName { get; }

    /// <summary>
    /// Выражение для присваивания.
    /// </summary>
    public Expression Expression { get; }

    /// <inheritdoc/>
    public override void Accept(IAstVisitor visitor)
    {
        visitor.VisitAssignmentStatement(this);
    }
}