using Ast.Declarations;
using Ast.Expressions;
using Ast.Statements;

namespace Ast;

/// <summary>
/// Интерфейс для обхода узлов абстрактного синтаксического дерева (паттерн Visitor).
/// </summary>
public interface IAstVisitor
{
    // Объявления
    void VisitFunctionDefinition(FunctionDefinition function);

    void VisitConstDeclaration(ConstDeclaration declaration);

    // Выражения
    void VisitNumberLiteral(NumberLiteral number);

    void VisitIdentifier(Identifier identifier);

    void VisitConstant(Constant constant);

    void VisitBinaryExpression(BinaryExpression binary);

    void VisitUnaryExpression(UnaryExpression unary);

    void VisitFunctionCall(FunctionCall call);

    // Инструкции
    void VisitVariableDeclaration(VariableDeclaration declaration);

    void VisitAssignmentStatement(AssignmentStatement assignment);

    void VisitInputStatement(InputStatement input);

    void VisitOutputStatement(OutputStatement output);

    void VisitIfStatement(IfStatement ifStatement);

    void VisitWhileStatement(WhileStatement whileStatement);

    void VisitForStatement(ForStatement forStatement);

    void VisitReturnStatement(ReturnStatement returnStatement);

    void VisitBlock(Block block);

    void VisitExpressionStatement(ExpressionStatement expression);
}