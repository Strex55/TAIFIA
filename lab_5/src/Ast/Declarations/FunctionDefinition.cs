using Ast.Statements;

namespace Ast.Declarations;

/// <summary>
/// Узел для определения функции (boss).
/// </summary>
public class FunctionDefinition : Declaration
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="FunctionDefinition"/>.
    /// </summary>
    /// <param name="name">Имя функции.</param>
    /// <param name="parameters">Список имен параметров.</param>
    /// <param name="body">Тело функции.</param>
    public FunctionDefinition(string name, IReadOnlyList<string> parameters, Block body)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        Body = body ?? throw new ArgumentNullException(nameof(body));
    }

    /// <summary>
    /// Имя функции.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Список имен параметров.
    /// </summary>
    public IReadOnlyList<string> Parameters { get; }

    /// <summary>
    /// Тело функции (блок кода).
    /// </summary>
    public Block Body { get; }

    /// <inheritdoc/>
    public override void Accept(IAstVisitor visitor)
    {
        visitor.VisitFunctionDefinition(this);
    }
}