namespace Ast;

/// <summary>
/// Базовый класс для всех узлов абстрактного синтаксического дерева.
/// </summary>
public abstract class AstNode
{
    /// <summary>
    /// Принимает посетителя (паттерн Visitor).
    /// </summary>
    /// <param name="visitor">Посетитель для обхода узла.</param>
    public abstract void Accept(IAstVisitor visitor);
}