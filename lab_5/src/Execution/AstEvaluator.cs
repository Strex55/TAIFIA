using Ast;
using Ast.Declarations;
using Ast.Expressions;
using Ast.Statements;

namespace Execution;

/// <summary>
/// Специальное исключение для возврата из функции.
/// </summary>
internal class FunctionReturnException : Exception
{
    public FunctionReturnException()
    {
    }

    public FunctionReturnException(string message)
        : base(message)
    {
    }

    public FunctionReturnException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public decimal ReturnValue { get; init; }
}

/// <summary>
/// Вычислитель AST
/// Вычисляет выражения и выполняет инструкции
/// </summary>
public class AstEvaluator : IAstVisitor
{
    private readonly Context context;
    private readonly IEnvironment environment;
    private readonly Dictionary<string, FunctionDefinition> functions = new();
    private Stack<decimal>? evaluationStack;
    private bool isInsideFunction;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AstEvaluator"/>.
    /// </summary>
    /// <param name="context">Контекст выполнения с областями видимости.</param>
    /// <param name="environment">Окружение для ввода/вывода.</param>
    public AstEvaluator(Context context, IEnvironment environment)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    /// <summary>
    /// Выполняет список узлов верхнего уровня программы.
    /// </summary>
    /// <param name="topLevelItems">Список элементов верхнего уровня.</param>
    public void EvaluateProgram(IReadOnlyList<AstNode> topLevelItems)
    {
        // Сначала обрабатываем все объявления (константы и функции)
        foreach (AstNode item in topLevelItems)
        {
            if (item is Declaration declaration)
            {
                declaration.Accept(this);
            }
        }

        // Затем выполняем инструкции
        foreach (AstNode item in topLevelItems)
        {
            if (item is Statement statement)
            {
                statement.Accept(this);
            }
        }
    }

    /// <summary>
    /// Вычисляет выражение стековым методом.
    /// </summary>
    /// <param name="expression">Выражение для вычисления.</param>
    /// <returns>Результат вычисления.</returns>
    public decimal EvaluateExpression(Expression expression)
    {
        Stack<decimal> stack = new Stack<decimal>();
        Stack<decimal>? savedStack = evaluationStack;
        evaluationStack = stack;

        try
        {
            expression.Accept(this);

            if (stack.Count != 1)
            {
                throw new InvalidOperationException($"Ошибка вычисления выражения: стек должен содержать одно значение, получено {stack.Count}");
            }

            return stack.Pop();
        }
        finally
        {
            evaluationStack = savedStack;
        }
    }

    // Реализация IAstVisitor
    public void VisitFunctionDefinition(FunctionDefinition function)
    {
        // Проверяем, не объявлена ли уже функция с таким именем
        if (functions.ContainsKey(function.Name))
        {
            throw new InvalidOperationException($"Функция '{function.Name}' уже объявлена");
        }

        // Сохраняем определение функции для последующего вызова
        functions[function.Name] = function;
    }

    public void VisitConstDeclaration(ConstDeclaration declaration)
    {
        decimal value = EvaluateExpression(declaration.Value);

        if (!context.TryDefineVariable(declaration.Name, value))
        {
            throw new InvalidOperationException($"Константа '{declaration.Name}' уже объявлена в этой области видимости");
        }
    }

    // Выражения - стековый метод вычисления
    public void VisitNumberLiteral(NumberLiteral number)
    {
        if (evaluationStack == null)
        {
            throw new InvalidOperationException("Стек вычисления не инициализирован");
        }

        evaluationStack.Push(number.Value);
    }

    public void VisitIdentifier(Identifier identifier)
    {
        if (evaluationStack == null)
        {
            throw new InvalidOperationException("Стек вычисления не инициализирован");
        }

        if (!context.TryGetVariable(identifier.Name, out decimal value))
        {
            throw new InvalidOperationException($"Неизвестная переменная: {identifier.Name}");
        }

        evaluationStack.Push(value);
    }

    public void VisitConstant(Constant constant)
    {
        if (evaluationStack == null)
        {
            throw new InvalidOperationException("Стек вычисления не инициализирован");
        }

        decimal value = constant.Type switch
        {
            Constant.ConstantType.Pi => 3.141592653589793m,
            Constant.ConstantType.E => 2.718281828459045m,
            _ => throw new InvalidOperationException($"Неизвестная константа: {constant.Type}")
        };

        evaluationStack.Push(value);
    }

    public void VisitBinaryExpression(BinaryExpression binary)
    {
        if (evaluationStack == null)
        {
            throw new InvalidOperationException("Стек вычисления не инициализирован");
        }

        // Вычисляем левый операнд
        binary.Left.Accept(this);

        // Вычисляем правый операнд
        binary.Right.Accept(this);

        // Берем два значения из стека
        decimal right = evaluationStack.Pop();
        decimal left = evaluationStack.Pop();

        // Применяем оператор и пушим результат
        decimal result = binary.Operator switch
        {
            BinaryExpression.BinaryOperator.Add => left + right,
            BinaryExpression.BinaryOperator.Subtract => left - right,
            BinaryExpression.BinaryOperator.Multiply => left * right,
            BinaryExpression.BinaryOperator.Divide => right != 0 ? left / right : throw new DivideByZeroException("Деление на ноль"),
            BinaryExpression.BinaryOperator.Modulo => right != 0 ? left % right : throw new DivideByZeroException("Остаток от деления на ноль"),
            BinaryExpression.BinaryOperator.Power => (decimal)Math.Pow((double)left, (double)right),
            BinaryExpression.BinaryOperator.Equal => left == right ? 1m : 0m,
            BinaryExpression.BinaryOperator.NotEqual => left != right ? 1m : 0m,
            BinaryExpression.BinaryOperator.LessThan => left < right ? 1m : 0m,
            BinaryExpression.BinaryOperator.LessThanOrEqual => left <= right ? 1m : 0m,
            BinaryExpression.BinaryOperator.GreaterThan => left > right ? 1m : 0m,
            BinaryExpression.BinaryOperator.GreaterThanOrEqual => left >= right ? 1m : 0m,
            BinaryExpression.BinaryOperator.LogicalAnd => (left != 0 && right != 0) ? 1m : 0m,
            BinaryExpression.BinaryOperator.LogicalOr => (left != 0 || right != 0) ? 1m : 0m,
            _ => throw new InvalidOperationException($"Неизвестный бинарный оператор: {binary.Operator}")
        };

        evaluationStack.Push(result);
    }

    public void VisitUnaryExpression(UnaryExpression unary)
    {
        if (evaluationStack == null)
        {
            throw new InvalidOperationException("Стек вычисления не инициализирован");
        }

        // Вычисляем операнд
        unary.Operand.Accept(this);

        // Берем значение из стека
        decimal operand = evaluationStack.Pop();

        // Применяем оператор и пушим результат
        decimal result = unary.Operator switch
        {
            UnaryExpression.UnaryOperator.Plus => operand,
            UnaryExpression.UnaryOperator.Minus => -operand,
            UnaryExpression.UnaryOperator.LogicalNot => operand == 0 ? 1m : 0m,
            _ => throw new InvalidOperationException($"Неизвестный унарный оператор: {unary.Operator}")
        };

        evaluationStack.Push(result);
    }

    public void VisitFunctionCall(FunctionCall call)
    {
        if (evaluationStack == null)
        {
            throw new InvalidOperationException("Стек вычисления не инициализирован");
        }

        // Вычисляем все аргументы и пушим их в стек
        foreach (Expression arg in call.Arguments)
        {
            arg.Accept(this);
        }

        // Собираем аргументы из стека (в обратном порядке)
        List<decimal> arguments = new List<decimal>();
        for (int i = call.Arguments.Count - 1; i >= 0; i--)
        {
            arguments.Add(evaluationStack.Pop());
        }

        arguments.Reverse();

        decimal result = 0m;

        // Проверяем встроенные функции
        if (IsBuiltinFunction(call.FunctionName))
        {
            result = BuiltinFunctions.Invoke(call.FunctionName, arguments);
        }
        else
        {
            // Проверяем пользовательские функции
            if (!functions.TryGetValue(call.FunctionName, out FunctionDefinition? function))
            {
                throw new InvalidOperationException($"Неизвестная функция: {call.FunctionName}");
            }

            // Проверяем количество аргументов
            if (call.Arguments.Count != function.Parameters.Count)
            {
                throw new InvalidOperationException(
                    $"Функция '{call.FunctionName}' ожидает {function.Parameters.Count} аргументов, получено {call.Arguments.Count}");
            }

            // Сохраняем текущий стек, так как выполнение функции может изменить его
            Stack<decimal>? savedStack = evaluationStack;

            // Создаем новую область видимости для функции
            context.PushScope();
            bool wasInsideFunction = isInsideFunction;
            isInsideFunction = true;
            bool hasReturned = false;

            try
            {
                for (int i = 0; i < function.Parameters.Count; i++)
                {
                    context.TryDefineVariable(function.Parameters[i], arguments[i]);
                }

                // Выполняем тело функции
                try
                {
                    function.Body.Accept(this);
                }
                catch (FunctionReturnException returnException)
                {
                    // Функция вернула значение через tank yu
                    result = returnException.ReturnValue;
                    hasReturned = true;
                }

                if (!hasReturned)
                {
                    throw new InvalidOperationException("Функция должна содержать 'tank yu'");
                }
            }
            finally
            {
                // Восстанавливаем стек на случай, если он был изменен при выполнении функции
                evaluationStack = savedStack;
                isInsideFunction = wasInsideFunction;
                context.PopScope();
            }
        }

        // Пушим результат в стек
        evaluationStack.Push(result);
    }

    // Инструкции
    public void VisitVariableDeclaration(VariableDeclaration declaration)
    {
        if (!context.TryDefineVariable(declaration.Name, 0m))
        {
            throw new InvalidOperationException($"Переменная '{declaration.Name}' уже объявлена в этой области видимости");
        }
    }

    public void VisitAssignmentStatement(AssignmentStatement assignment)
    {
        decimal value = EvaluateExpression(assignment.Expression);

        if (!context.TryAssignVariable(assignment.VariableName, value))
        {
            throw new InvalidOperationException($"Неизвестная переменная: {assignment.VariableName}");
        }
    }

    public void VisitInputStatement(InputStatement input)
    {
        decimal value = environment.ReadNumber();

        // Пытаемся присвоить существующей переменной
        if (!context.TryAssignVariable(input.VariableName, value))
        {
            // Если переменная не существует, создаем её
            if (!context.TryDefineVariable(input.VariableName, value))
            {
                throw new InvalidOperationException($"Не удалось создать переменную: {input.VariableName}");
            }
        }
    }

    public void VisitOutputStatement(OutputStatement output)
    {
        decimal value = EvaluateExpression(output.Expression);
        environment.WriteNumber(value);
    }

    public void VisitIfStatement(IfStatement ifStatement)
    {
        decimal condition = EvaluateExpression(ifStatement.Condition);

        try
        {
            if (condition != 0)
            {
                ifStatement.ThenBlock.Accept(this);
            }
            else if (ifStatement.ElseBlock != null)
            {
                ifStatement.ElseBlock.Accept(this);
            }
        }
        catch (FunctionReturnException)
        {
            // Пробрасываем исключение возврата из функции наверх
            throw;
        }
    }

    public void VisitWhileStatement(WhileStatement whileStatement)
    {
        while (true)
        {
            decimal condition = EvaluateExpression(whileStatement.Condition);

            if (condition == 0)
            {
                break;
            }

            try
            {
                whileStatement.Body.Accept(this);
            }
            catch (FunctionReturnException)
            {
                // Пробрасываем исключение возврата из функции наверх
                throw;
            }
        }
    }

    public void VisitForStatement(ForStatement forStatement)
    {
        // Вычисляем начальное и конечное значения
        decimal start = EvaluateExpression(forStatement.StartExpression);
        decimal end = EvaluateExpression(forStatement.EndExpression);

        // Создаем новую область видимости для переменной цикла
        context.PushScope();

        try
        {
            // Объявляем переменную цикла с начальным значением
            if (!context.TryDefineVariable(forStatement.VariableName, start))
            {
                throw new InvalidOperationException(
                    $"Переменная '{forStatement.VariableName}' уже объявлена в этой области видимости");
            }

            // Выполняем цикл: от start до end включительно
            decimal current = start;
            while (current <= end)
            {
                // Обновляем значение переменной цикла
                if (!context.TryAssignVariable(forStatement.VariableName, current))
                {
                    throw new InvalidOperationException(
                        $"Не удалось присвоить значение переменной цикла '{forStatement.VariableName}'");
                }

                try
                {
                    forStatement.Body.Accept(this);
                }
                catch (FunctionReturnException)
                {
                    // Пробрасываем исключение возврата из функции наверх
                    throw;
                }

                current++;
            }
        }
        finally
        {
            // Удаляем область видимости переменной цикла
            context.PopScope();
        }
    }

    public void VisitReturnStatement(ReturnStatement returnStatement)
    {
        if (!isInsideFunction)
        {
            throw new InvalidOperationException("'tank yu' может использоваться только внутри функции");
        }

        // Вычисляем выражение и выбрасываем специальное исключение для возврата из функции
        decimal returnValue = EvaluateExpression(returnStatement.Expression);
        throw new FunctionReturnException { ReturnValue = returnValue };
    }

    public void VisitBlock(Block block)
    {
        context.PushScope();

        try
        {
            foreach (Statement statement in block.Statements)
            {
                statement.Accept(this);
            }
        }
        catch (FunctionReturnException)
        {
            // Пробрасываем исключение возврата из функции наверх
            throw;
        }
        finally
        {
            context.PopScope();
        }
    }

    public void VisitExpressionStatement(ExpressionStatement expression)
    {
        // Вычисляем выражение и игнорируем результат
        EvaluateExpression(expression.Expression);
    }

    private static bool IsBuiltinFunction(string name)
    {
        return name == "muak" || name == "miniboss" || name == "bigboss";
    }
}