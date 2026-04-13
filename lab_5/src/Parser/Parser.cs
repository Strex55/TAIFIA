using System.Globalization;
using Ast;
using Ast.Declarations;
using Ast.Expressions;
using Ast.Statements;
using Execution;
using Lexer;

namespace Parser;

/// <summary>
/// Рекурсивный спускающийся парсер языка Minion#.
/// Выражения — по грамматике `docs/specification/expressions-grammar.md`,
/// верхний уровень — по `docs/specification/top-level-grammar.md`.
/// </summary>
public class Parser
{
    private readonly TokenStream tokenStream;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Parser"/>.
    /// </summary>
    /// <param name="tokenStream">Поток токенов для разбора.</param>
    public Parser(TokenStream tokenStream)
    {
        this.tokenStream = tokenStream ?? throw new ArgumentNullException(nameof(tokenStream));
    }

    /// <summary>
    /// program = "bello!" , { top-level-item } ;
    /// Парсит программу и возвращает список узлов AST верхнего уровня.
    /// </summary>
    /// <param name="code">Исходный код программы.</param>
    /// <returns>Список узлов AST верхнего уровня.</returns>
    public static IReadOnlyList<AstNode> ParseProgram(string code)
    {
        Parser parser = new(new TokenStream(code));
        return parser.ParseProgramInternal();
    }

    /// <summary>
    /// Парсит выражение и возвращает AST узел.
    /// </summary>
    /// <param name="code">Исходный код выражения.</param>
    /// <returns>AST узел выражения.</returns>
    public static Expression ParseExpression(string code)
    {
        TokenStream stream = new TokenStream(code);
        Parser parser = new Parser(stream);
        Expression result = parser.ParseExpression();

        Token remainingToken = stream.Peek();
        if (remainingToken.Type != TokenType.EndOfFile)
        {
            throw new InvalidOperationException($"Неожиданный токен после выражения: {remainingToken}");
        }

        return result;
    }

    private IReadOnlyList<AstNode> ParseProgramInternal()
    {
        Expect(TokenType.Bello, "Ожидался старт программы 'bello!'");

        List<AstNode> topLevelItems = new List<AstNode>();

        while (tokenStream.Peek().Type != TokenType.EndOfFile)
        {
            AstNode item = ParseTopLevelItem();
            topLevelItems.Add(item);
        }

        return topLevelItems;
    }

    /// <summary>
    /// top-level-item = const-declaration | function-definition | statement ;
    /// </summary>
    private AstNode ParseTopLevelItem()
    {
        Token token = tokenStream.Peek();
        return token.Type switch
        {
            TokenType.Trusela => ParseConstDeclaration(),
            TokenType.Boss => ParseFunctionDefinition(),
            _ => ParseStatement()
        };
    }

    /// <summary>
    /// const-declaration = "trusela" , identifier , "Papaya" , const-value , "naidu!" ;
    /// const-value = number-literal | constant ;
    /// </summary>
    private ConstDeclaration ParseConstDeclaration()
    {
        tokenStream.Advance(); // trusela
        Token constName = Expect(TokenType.Identifier, "Ожидалось имя константы после 'trusela'");
        ExpectIdentifierLexeme("Papaya", "Ожидался тип Papaya в объявлении константы");

        Expression value = ParseConstValue();

        Expect(TokenType.Naidu, "Ожидался разделитель 'naidu!'");

        return new ConstDeclaration(constName.Lexeme, value);
    }

    /// <summary>
    /// Парсит значение константы (number-literal | constant).
    /// </summary>
    private Expression ParseConstValue()
    {
        Token valueToken = tokenStream.Peek();

        return valueToken.Type switch
        {
            TokenType.NumberLiteral => ParseNumberLiteral(),
            TokenType.Identifier => ParseConstantIdentifier(valueToken.Lexeme),
            _ => throw new InvalidOperationException("Ожидалось константное значение (число или belloPi/belloE)")
        };
    }

    private NumberLiteral ParseNumberLiteral()
    {
        Token token = tokenStream.Peek();
        tokenStream.Advance();
        decimal value = decimal.Parse(token.Lexeme, CultureInfo.InvariantCulture);
        return new NumberLiteral(value);
    }

    private Expression ParseConstantIdentifier(string identifier)
    {
        tokenStream.Advance();

        return identifier.Equals("belloPi", StringComparison.OrdinalIgnoreCase)
            ? new Constant(Constant.ConstantType.Pi)
            : identifier.Equals("belloE", StringComparison.OrdinalIgnoreCase)
                ? new Constant(Constant.ConstantType.E)
                : throw new InvalidOperationException("Ожидалось константное значение (число или belloPi/belloE)");
    }

    /// <summary>
    /// variable-declaration = "poop" , identifier , "Papaya" , "naidu!" ;
    /// </summary>
    private VariableDeclaration ParseVarDeclaration()
    {
        tokenStream.Advance(); // poop
        Token name = Expect(TokenType.Identifier, "Ожидалось имя переменной после 'poop'");
        ExpectIdentifierLexeme("Papaya", "Ожидался тип Papaya в объявлении переменной");
        Expect(TokenType.Naidu, "Ожидался разделитель 'naidu!'");

        return new VariableDeclaration(name.Lexeme);
    }

    /// <summary>
    /// statement =
    ///     variable-declaration
    ///   | assignment-statement
    ///   | input-statement
    ///   | output-statement
    ///   | if-statement
    ///   | while-statement
    ///   | for-statement
    ///   | return-statement
    ///   | expression-statement
    ///   , "naidu!" ;
    /// </summary>
    private Statement ParseStatement()
    {
        Token token = tokenStream.Peek();

        // Если встретили EndOfFile при попытке парсить statement это может означать незакрытый блок
        if (token.Type == TokenType.EndOfFile)
        {
            throw new InvalidOperationException("Ожидался конец блока 'stopa', но достигнут конец файла");
        }

        return token.Type switch
        {
            TokenType.Poop => ParseVarDeclaration(),
            TokenType.Identifier => ParseAssignmentOrExpressionStatement(),
            TokenType.Tulalilloo => ParseOutput(),
            TokenType.Guoleila => ParseInput(),
            TokenType.BiDo => ParseIf(),
            TokenType.Kemari => ParseWhile(),
            TokenType.Again => ParseFor(),
            TokenType.Tank => ParseReturn(),
            _ => throw new InvalidOperationException($"Неожиданный токен в инструкции: {token}")
        };
    }

    /// <summary>
    /// assignment-statement = identifier , "lumai" , expression , "naidu!" ;
    /// expression-statement = expression , "naidu!" ;
    /// </summary>
    private Statement ParseAssignmentOrExpressionStatement()
    {
        Token ident = tokenStream.Peek();
        tokenStream.Advance();

        Token nextToken = tokenStream.Peek();
        if (nextToken.Type == TokenType.Operator && nextToken.Lexeme == "lumai")
        {
            tokenStream.Advance(); // lumai
            Expression value = ParseExpression();
            Expect(TokenType.Naidu, "Ожидался разделитель 'naidu!'");
            return new AssignmentStatement(ident.Lexeme, value);
        }

        // Expression statement - нужно вернуть токен обратно и парсить как выражение
        tokenStream.Back();
        Expression expr = ParseExpression();
        Expect(TokenType.Naidu, "Ожидался разделитель 'naidu!'");
        return new ExpressionStatement(expr);
    }

    /// <summary>
    /// input-statement = "guoleila" , "(", identifier , ")", "naidu!" ;
    /// </summary>
    private InputStatement ParseInput()
    {
        tokenStream.Advance(); // guoleila
        ExpectDelimiter("(");
        Token name = Expect(TokenType.Identifier, "Ожидалось имя переменной для ввода");
        ExpectDelimiter(")");
        Expect(TokenType.Naidu, "Ожидался разделитель 'naidu!'");

        return new InputStatement(name.Lexeme);
    }

    /// <summary>
    /// output-statement = "tulalilloo" , "ti" , "amo" , "(" , expression , ")" , "naidu!" ;
    /// </summary>
    private OutputStatement ParseOutput()
    {
        tokenStream.Advance(); // tulalilloo
        Expect(TokenType.Ti, "Ожидалось 'ti'");
        Expect(TokenType.Amo, "Ожидалось 'amo'");
        ExpectDelimiter("(");
        Expression expr = ParseExpression();
        ExpectDelimiter(")");
        Expect(TokenType.Naidu, "Ожидался разделитель 'naidu!'");

        return new OutputStatement(expr);
    }

    /// <summary>
    /// if-statement = "bi-do" , "(" , expression , ")" , block , [ "uh-oh" , block ] ;
    /// </summary>
    private IfStatement ParseIf()
    {
        tokenStream.Advance(); // bi-do
        ExpectDelimiter("(");
        Expression condition = ParseExpression();
        ExpectDelimiter(")");

        Block thenBlock = ParseBlock();

        Block? elseBlock = null;
        if (tokenStream.Peek().Type == TokenType.UhOh)
        {
            tokenStream.Advance();
            elseBlock = ParseBlock();
        }

        return new IfStatement(condition, thenBlock, elseBlock);
    }

    /// <summary>
    /// while-statement = "kemari" , "(" , expression , ")" , block ;
    /// </summary>
    private WhileStatement ParseWhile()
    {
        tokenStream.Advance(); // kemari
        ExpectDelimiter("(");
        Expression condition = ParseExpression();
        ExpectDelimiter(")");
        Block body = ParseBlock();

        return new WhileStatement(condition, body);
    }

    /// <summary>
    /// for-statement = "again" , "(" , identifier , "=" , expression , "to" , expression , ")" , block ;
    /// </summary>
    private ForStatement ParseFor()
    {
        tokenStream.Advance(); // again
        ExpectDelimiter("(");
        
        Token variableToken = Expect(TokenType.Identifier, "Ожидалось имя переменной цикла");
        string variableName = variableToken.Lexeme;
        
        // Проверяем оператор присваивания "="
        Token assignToken = tokenStream.Peek();
        if (assignToken.Type != TokenType.Delimiter || assignToken.Lexeme != "=")
        {
            throw new InvalidOperationException("Ожидался оператор '=' после имени переменной");
        }

        tokenStream.Advance(); // пропускаем "="

        Expression startExpression = ParseExpression();

        // Проверяем ключевое слово "to"
        Token toToken = tokenStream.Peek();
        if (toToken.Type != TokenType.Identifier || toToken.Lexeme != "to")
        {
            throw new InvalidOperationException("Ожидалось ключевое слово 'to' после начального выражения");
        }

        tokenStream.Advance(); // пропускаем "to"

        Expression endExpression = ParseExpression();
        ExpectDelimiter(")");

        Block body = ParseBlock();

        return new ForStatement(variableName, startExpression, endExpression, body);
    }

    /// <summary>
    /// return-statement = "tank" , "yu" , expression , "naidu!" ;
    /// </summary>
    private ReturnStatement ParseReturn()
    {
        tokenStream.Advance(); // tank
        Expect(TokenType.Yu, "Ожидалось 'yu' после 'tank'");
        Expression expr = ParseExpression();
        Expect(TokenType.Naidu, "Ожидался разделитель 'naidu!'");
        return new ReturnStatement(expr);
    }

    /// <summary>
    /// function-definition = "boss" , identifier , "Papaya" , "(" , [ parameter-list ] , ")" , block ;
    /// parameter-list = identifier , { \",\" , identifier } ;
    /// </summary>
    private FunctionDefinition ParseFunctionDefinition()
    {
        tokenStream.Advance(); // boss
        Token functionName = Expect(TokenType.Identifier, "Ожидалось имя функции после 'boss'");
        ExpectIdentifierLexeme("Papaya", "Ожидался тип Papaya у функции");
        ExpectDelimiter("(");

        List<string> parameters = new();
        Token nextToken = tokenStream.Peek();
        if (nextToken.Type != TokenType.Delimiter || nextToken.Lexeme != ")")
        {
            Token param = Expect(TokenType.Identifier, "Ожидалось имя параметра");
            parameters.Add(param.Lexeme);
            while (true)
            {
                Token token = tokenStream.Peek();
                if (token.Type == TokenType.Delimiter && token.Lexeme == ",")
                {
                    tokenStream.Advance();
                    param = Expect(TokenType.Identifier, "Ожидалось имя параметра");
                    parameters.Add(param.Lexeme);
                }
                else
                {
                    break;
                }
            }
        }

        ExpectDelimiter(")");

        Block body = ParseBlock();

        return new FunctionDefinition(functionName.Lexeme, parameters, body);
    }

    /// <summary>
    /// block = "oca!" , { statement } , "stopa" ;
    /// </summary>
    private Block ParseBlock()
    {
        Expect(TokenType.Oca, "Ожидался старт блока 'oca!'");

        List<Statement> statements = new List<Statement>();

        // Парсим statements до тех пор, пока не встретим 'stopa'
        while (tokenStream.Peek().Type != TokenType.Stopa)
        {
            Token currentToken = tokenStream.Peek();

            // Если встретили EndOfFile, это ошибка - блок не закрыт
            if (currentToken.Type == TokenType.EndOfFile)
            {
                throw new InvalidOperationException("Ожидался конец блока 'stopa', но достигнут конец файла");
            }

            Statement stmt = ParseStatement();
            statements.Add(stmt);
        }

        // Должны быть на токене 'stopa'
        Expect(TokenType.Stopa, "Ожидался конец блока 'stopa'");
        return new Block(statements);
    }

    /// <summary>
    /// Разбирает выражение верхнего уровня.
    /// Правила:
    ///     expression = logical-or-expression ;
    /// </summary>
    private Expression ParseExpression()
    {
        return ParseLogicalOrExpression();
    }

    /// <summary>
    /// Разбирает логическое ИЛИ выражение.
    /// Правила:
    ///     logical-or-expression = logical-and-expression , { "bo-ca" , logical-and-expression } ;
    /// </summary>
    private Expression ParseLogicalOrExpression()
    {
        Expression left = ParseLogicalAndExpression();

        while (IsOperator("bo-ca"))
        {
            tokenStream.Advance();
            Expression right = ParseLogicalAndExpression();
            left = new BinaryExpression(left, BinaryExpression.BinaryOperator.LogicalOr, right);
        }

        return left;
    }

    /// <summary>
    /// Разбирает логическое И выражение.
    /// Правила:
    ///     logical-and-expression = logical-not-expression , { "tropa" , logical-not-expression } ;
    /// </summary>
    private Expression ParseLogicalAndExpression()
    {
        Expression left = ParseLogicalNotExpression();

        while (IsOperator("tropa"))
        {
            tokenStream.Advance();
            Expression right = ParseLogicalNotExpression();
            left = new BinaryExpression(left, BinaryExpression.BinaryOperator.LogicalAnd, right);
        }

        return left;
    }

    /// <summary>
    /// Разбирает логическое НЕ выражение.
    /// Правила:
    ///     logical-not-expression = equality-expression
    ///                             | "makoroni" , logical-not-expression ;
    /// </summary>
    private Expression ParseLogicalNotExpression()
    {
        if (IsOperator("makoroni"))
        {
            tokenStream.Advance();
            Expression operand = ParseLogicalNotExpression();
            return new UnaryExpression(UnaryExpression.UnaryOperator.LogicalNot, operand);
        }

        return ParseEqualityExpression();
    }

    /// <summary>
    /// Разбирает выражение равенства и неравенства.
    /// Правила:
    ///     equality-expression = relational-expression , { ("con" | "nocon") , relational-expression } ;
    /// </summary>
    private Expression ParseEqualityExpression()
    {
        Expression left = ParseRelationalExpression();

        while (true)
        {
            Token token = tokenStream.Peek();
            if (token.Type != TokenType.Operator)
            {
                break;
            }

            if (token.Lexeme == "con")
            {
                tokenStream.Advance();
                Expression right = ParseRelationalExpression();
                left = new BinaryExpression(left, BinaryExpression.BinaryOperator.Equal, right);
            }
            else if (token.Lexeme == "nocon")
            {
                tokenStream.Advance();
                Expression right = ParseRelationalExpression();
                left = new BinaryExpression(left, BinaryExpression.BinaryOperator.NotEqual, right);
            }
            else
            {
                break;
            }
        }

        return left;
    }

    /// <summary>
    /// Разбирает выражение сравнения.
    /// Правила:
    ///     relational-expression = additive-expression ,
    ///                             { ("looka too" , [ "con" ] | "la" , [ "con" ]) , additive-expression } ;
    /// </summary>
    private Expression ParseRelationalExpression()
    {
        Expression left = ParseAdditiveExpression();

        while (true)
        {
            Token token = tokenStream.Peek();
            if (token.Type != TokenType.Operator)
            {
                break;
            }

            BinaryExpression.BinaryOperator? binaryOp = null;

            if (token.Lexeme == "lacon")
            {
                tokenStream.Advance();
                binaryOp = BinaryExpression.BinaryOperator.LessThanOrEqual;
            }
            else if (token.Lexeme == "la")
            {
                tokenStream.Advance();
                bool hasCon = IsOperator("con");
                if (hasCon)
                {
                    tokenStream.Advance();
                }

                binaryOp = hasCon
                    ? BinaryExpression.BinaryOperator.LessThanOrEqual
                    : BinaryExpression.BinaryOperator.LessThan;
            }
            else if (token.Lexeme == "looka")
            {
                tokenStream.Advance();
                if (!IsOperator("too"))
                {
                    throw new InvalidOperationException("После 'looka' ожидалось 'too'");
                }

                tokenStream.Advance();
                bool hasCon = IsOperator("con");
                if (hasCon)
                {
                    tokenStream.Advance();
                }

                binaryOp = hasCon
                    ? BinaryExpression.BinaryOperator.GreaterThanOrEqual
                    : BinaryExpression.BinaryOperator.GreaterThan;
            }
            else
            {
                break;
            }

            Expression right = ParseAdditiveExpression();
            left = new BinaryExpression(left, binaryOp.Value, right);
        }

        return left;
    }

    /// <summary>
    /// Разбирает выражение сложения и вычитания.
    /// Правила:
    ///     additive-expression = multiplicative-expression ,
    ///                           { ("melomo" | "flavuk") , multiplicative-expression } ;
    /// </summary>
    private Expression ParseAdditiveExpression()
    {
        Expression left = ParseMultiplicativeExpression();

        while (true)
        {
            Token token = tokenStream.Peek();
            if (token.Type != TokenType.Operator)
            {
                break;
            }

            if (token.Lexeme == "melomo")
            {
                tokenStream.Advance();
                Expression right = ParseMultiplicativeExpression();
                left = new BinaryExpression(left, BinaryExpression.BinaryOperator.Add, right);
            }
            else if (token.Lexeme == "flavuk")
            {
                tokenStream.Advance();
                Expression right = ParseMultiplicativeExpression();
                left = new BinaryExpression(left, BinaryExpression.BinaryOperator.Subtract, right);
            }
            else
            {
                break;
            }
        }

        return left;
    }

    /// <summary>
    /// Разбирает выражение умножения, деления и остатка.
    /// Правила:
    ///     multiplicative-expression = unary-expression ,
    ///                                 { ("dibotada" | "poopaye" | "pado") , unary-expression } ;
    /// </summary>
    private Expression ParseMultiplicativeExpression()
    {
        Expression left = ParseUnaryExpression();

        while (true)
        {
            Token token = tokenStream.Peek();
            if (token.Type != TokenType.Operator)
            {
                break;
            }

            BinaryExpression.BinaryOperator? binaryOp = token.Lexeme switch
            {
                "dibotada" => BinaryExpression.BinaryOperator.Multiply,
                "poopaye" => BinaryExpression.BinaryOperator.Divide,
                "pado" => BinaryExpression.BinaryOperator.Modulo,
                _ => null
            };

            if (binaryOp == null)
            {
                break;
            }

            tokenStream.Advance();
            Expression right = ParseUnaryExpression();
            left = new BinaryExpression(left, binaryOp.Value, right);
        }

        return left;
    }

    /// <summary>
    /// Разбирает унарное выражение.
    /// Правила:
    ///     unary-expression = power-expression
    ///                      | "melomo" , unary-expression
    ///                      | "flavuk" , unary-expression;
    /// </summary>
    private Expression ParseUnaryExpression()
    {
        if (IsOperator("melomo"))
        {
            tokenStream.Advance();
            Expression operand = ParseUnaryExpression();
            return new UnaryExpression(UnaryExpression.UnaryOperator.Plus, operand);
        }

        if (IsOperator("flavuk"))
        {
            tokenStream.Advance();
            Expression operand = ParseUnaryExpression();
            return new UnaryExpression(UnaryExpression.UnaryOperator.Minus, operand);
        }

        return ParsePowerExpression();
    }

    /// <summary>
    /// Разбирает выражение возведения в степень.
    /// Правила:
    ///     power-expression = primary-expression , [ "beedo" , power-expression ] ;
    /// </summary>
    private Expression ParsePowerExpression()
    {
        Expression left = ParsePrimaryExpression();

        if (IsOperator("beedo"))
        {
            tokenStream.Advance();
            Expression right = ParsePowerExpression();
            return new BinaryExpression(left, BinaryExpression.BinaryOperator.Power, right);
        }

        return left;
    }

    /// <summary>
    /// Разбирает первичное выражение.
    /// Правила:
    ///     primary-expression = number-literal
    ///                        | identifier
    ///                        | constant
    ///                        | "(" , expression , ")"
    ///                        | function-call ;
    /// </summary>
    private Expression ParsePrimaryExpression()
    {
        Token token = tokenStream.Peek();

        return token.Type switch
        {
            TokenType.NumberLiteral => ParseNumberLiteral(),
            TokenType.Da => ParseBooleanLiteral(1),
            TokenType.No => ParseBooleanLiteral(0),
            TokenType.StringLiteral => ParseStringLiteral(),
            TokenType.Identifier => ParseIdentifierOrFunctionCall(token.Lexeme),
            _ when token.Type == TokenType.Delimiter && token.Lexeme == "(" => ParseParenthesizedExpression(),
            _ => throw new InvalidOperationException($"Неожиданный токен: {token}")
        };
    }

    private NumberLiteral ParseBooleanLiteral(decimal value)
    {
        tokenStream.Advance();
        return new NumberLiteral(value);
    }

    private Expression ParseStringLiteral()
    {
        tokenStream.Advance();
        throw new NotImplementedException("Вычисление строковых литералов не реализовано");
    }

    private Expression ParseIdentifierOrFunctionCall(string name)
    {
        tokenStream.Advance();

        // Проверяем, является ли это константой
        if (name.Equals("belloPi", StringComparison.OrdinalIgnoreCase))
        {
            return new Constant(Constant.ConstantType.Pi);
        }

        if (name.Equals("belloE", StringComparison.OrdinalIgnoreCase))
        {
            return new Constant(Constant.ConstantType.E);
        }

        // Проверяем, является ли это вызовом функции
        Token nextToken = tokenStream.Peek();
        if (nextToken.Type == TokenType.Delimiter && nextToken.Lexeme == "(")
        {
            return ParseFunctionCall(name);
        }

        // Иначе это переменная
        return new Identifier(name);
    }

    private Expression ParseParenthesizedExpression()
    {
        tokenStream.Advance(); // пропускаем '('
        Expression result = ParseExpression();
        ExpectDelimiter(")");
        return result;
    }

    /// <summary>
    /// Разбирает вызов функции.
    /// Правила:
    ///     function-call = identifier , "(" , [ argument-list ] , ")" ;
    ///     argument-list = expression , { "," , expression } ;
    /// </summary>
    private FunctionCall ParseFunctionCall(string functionName)
    {
        // Уже прочитали идентификатор, открывающая скобка уже проверена в ParsePrimaryExpression
        tokenStream.Advance(); // пропускаем '('

        List<Expression> arguments = new List<Expression>();

        // Если сразу закрывающая скобка, то аргументов нет
        Token nextToken = tokenStream.Peek();
        if (nextToken.Type == TokenType.Delimiter && nextToken.Lexeme == ")")
        {
            tokenStream.Advance();
            return new FunctionCall(functionName, arguments);
        }

        // Читаем первый аргумент
        arguments.Add(ParseExpression());

        // Читаем остальные аргументы
        while (true)
        {
            Token token = tokenStream.Peek();
            if (token.Type == TokenType.Delimiter && token.Lexeme == ",")
            {
                tokenStream.Advance();
                arguments.Add(ParseExpression());
            }
            else if (token.Type == TokenType.Delimiter && token.Lexeme == ")")
            {
                tokenStream.Advance();
                break;
            }
            else
            {
                throw new InvalidOperationException("Ожидалась закрывающая скобка ')' или запятая ','");
            }
        }

        return new FunctionCall(functionName, arguments);
    }

    /// <summary>
    /// Проверяет, является ли текущий токен оператором с заданным именем.
    /// </summary>
    private bool IsOperator(string operatorName)
    {
        Token token = tokenStream.Peek();
        return token.Type == TokenType.Operator && token.Lexeme == operatorName;
    }

    private Token Expect(TokenType type, string message)
    {
        Token token = tokenStream.Peek();
        if (token.Type != type)
        {
            // Если ожидался определенный токен, но встречен EndOfFile, это может означать незакрытый блок
            if (token.Type == TokenType.EndOfFile)
            {
                throw new InvalidOperationException("Ожидался конец блока 'stopa', но достигнут конец файла");
            }

            throw new InvalidOperationException(message);
        }

        tokenStream.Advance();
        return token;
    }

    private void ExpectIdentifierLexeme(string lexeme, string message)
    {
        Token token = tokenStream.Peek();
        if (token.Type != TokenType.Identifier || token.Lexeme != lexeme)
        {
            throw new InvalidOperationException(message);
        }

        tokenStream.Advance();
    }

    private void ExpectDelimiter(string lexeme)
    {
        Token token = tokenStream.Peek();
        if (token.Type != TokenType.Delimiter || token.Lexeme != lexeme)
        {
            string actualToken = token.Type == TokenType.EndOfFile 
                ? "конец файла" 
                : $"'{token.Lexeme}' (тип: {token.Type})";
            
            // Получаем имя метода, который вызвал ExpectDelimiter
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(skipFrames: 1);
            string callingMethod = stackTrace.GetFrame(0)?.GetMethod()?.Name ?? "unknown";
            
            throw new InvalidOperationException(
                $"Ожидался разделитель '{lexeme}', но встречен: {actualToken}. " +
                $"Позиция токена: {token.Position}. " +
                $"Вызвано из метода: {callingMethod}");
        }

        tokenStream.Advance();
    }
}