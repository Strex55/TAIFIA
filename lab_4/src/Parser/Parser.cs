using System.Globalization;
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
    private readonly Context context;
    private readonly IEnvironment? environment;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Parser"/>.
    /// </summary>
    /// <param name="tokenStream">Поток токенов для разбора.</param>
    /// <param name="context">Контекст выполнения с областями видимости (опционально).</param>
    /// <param name="environment">Окружение для выполнения программы (опционально).</param>
    public Parser(TokenStream tokenStream, Context? context = null, IEnvironment? environment = null)
    {
        this.tokenStream = tokenStream ?? throw new ArgumentNullException(nameof(tokenStream));
        this.context = context ?? new Context();
        this.environment = environment;
    }

    /// <summary>
    /// program = "bello!" , { top-level-item } ;
    /// Выполняет программу с новым контекстом.
    /// </summary>
    /// <param name="code">Исходный код программы.</param>
    /// <param name="environment">Окружение для выполнения программы.</param>
    public static void ParseProgram(string code, IEnvironment? environment = null)
    {
        ParseProgram(code, new Context(), environment);
    }

    /// <summary>
    /// program = "bello!" , { top-level-item } ;
    /// Выполняет программу с использованием указанного контекста и окружения.
    /// </summary>
    /// <param name="code">Исходный код программы.</param>
    /// <param name="context">Контекст выполнения с областями видимости.</param>
    /// <param name="environment">Окружение для выполнения программы.</param>
    public static void ParseProgram(string code, Context context, IEnvironment? environment = null)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        Parser parser = new(new TokenStream(code), context, environment);
        parser.ParseProgramInternal();
    }

    /// <summary>
    /// Выполняет синтаксический разбор и вычисление выражения.
    /// </summary>
    /// <param name="code">Исходный код выражения.</param>
    /// <param name="variables">Словарь значений переменных</param>
    /// <returns>Результат вычисления выражения.</returns>
    public decimal EvaluateExpression(string code, Dictionary<string, decimal>? variables = null)
    {
        Context ctx = new Context();
        if (variables != null)
        {
            foreach (KeyValuePair<string, decimal> kvp in variables)
            {
                ctx.TryDefineVariable(kvp.Key, kvp.Value);
            }
        }

        TokenStream stream = new TokenStream(code);
        Parser parser = new Parser(stream, ctx);
        decimal result = parser.ParseExpression();

        // Проверяем, что все токены обработаны
        Token remainingToken = stream.Peek();
        if (remainingToken.Type != TokenType.EndOfFile)
        {
            throw new InvalidOperationException($"Неожиданный токен после выражения: {remainingToken}");
        }

        return result;
    }

    private void ParseProgramInternal()
    {
        Expect(TokenType.Bello, "Ожидался старт программы 'bello!'");

        while (tokenStream.Peek().Type != TokenType.EndOfFile)
        {
            ParseTopLevelItem();
        }
    }

    /// <summary>
    /// top-level-item = const-declaration | statement ;
    /// </summary>
    private bool ParseTopLevelItem()
    {
        Token token = tokenStream.Peek();
        return token.Type switch
        {
            TokenType.Trusela => ParseConstDeclaration(),
            _ => ParseStatement()
        };
    }

    /// <summary>
    /// const-declaration = "trusela" , identifier , "Papaya" , const-value , "naidu!" ;
    /// const-value = number-literal | constant ;
    /// </summary>
    private bool ParseConstDeclaration()
    {
        tokenStream.Advance(); // trusela
        Token constName = Expect(TokenType.Identifier, "Ожидалось имя константы после 'trusela'");
        ExpectIdentifierLexeme("Papaya", "Ожидался тип Papaya в объявлении константы");

        Token valueToken = tokenStream.Peek();
        decimal constValue;

        if (valueToken.Type == TokenType.NumberLiteral)
        {
            tokenStream.Advance();
            constValue = decimal.Parse(valueToken.Lexeme, CultureInfo.InvariantCulture);
        }
        else if (valueToken.Type == TokenType.Identifier)
        {
            if (!TryGetConstantValue(valueToken.Lexeme, out constValue))
            {
                throw new InvalidOperationException("Ожидалось константное значение (число или belloPi/belloE)");
            }

            tokenStream.Advance();
        }
        else
        {
            throw new InvalidOperationException("Ожидалось константное значение (число или belloPi/belloE)");
        }

        Expect(TokenType.Naidu, "Ожидался разделитель 'naidu!'");

        // Сохраняем константу в контекст
        if (environment != null)
        {
            if (!context.TryDefineVariable(constName.Lexeme, constValue))
            {
                throw new InvalidOperationException($"Константа '{constName.Lexeme}' уже объявлена в этой области видимости");
            }
        }

        return false;
    }

    /// <summary>
    /// variable-declaration = "poop" , identifier , "Papaya" , "naidu!" ;
    /// </summary>
    private bool ParseVarDeclaration()
    {
        tokenStream.Advance(); // poop
        Token name = Expect(TokenType.Identifier, "Ожидалось имя переменной после 'poop'");
        ExpectIdentifierLexeme("Papaya", "Ожидался тип Papaya в объявлении переменной");
        Expect(TokenType.Naidu, "Ожидался разделитель 'naidu!'");

        DefineVariable(name.Lexeme);
        return false;
    }

    /// <summary>
    /// statement =
    ///     variable-declaration
    ///   | assignment-statement
    ///   | input-statement
    ///   | output-statement
    ///   | expression-statement
    ///   , "naidu!" ;
    /// </summary>
    private bool ParseStatement()
    {
        Token token = tokenStream.Peek();

        return token.Type switch
        {
            TokenType.Poop => ParseVarDeclaration(),
            TokenType.Identifier => ParseAssignmentOrExpressionStatement(),
            TokenType.Tulalilloo => ParseOutput(),
            TokenType.Guoleila => ParseInput(),
            _ => throw new InvalidOperationException($"Неожиданный токен в инструкции: {token}")
        };
    }

    /// <summary>
    /// assignment-statement = identifier , "lumai" , expression , "naidu!" ;
    /// expression-statement = expression , "naidu!" ;
    /// </summary>
    private bool ParseAssignmentOrExpressionStatement()
    {
        Token ident = tokenStream.Peek();
        tokenStream.Advance();

        if (tokenStream.Peek().Type == TokenType.Operator && tokenStream.Peek().Lexeme == "lumai")
        {
            tokenStream.Advance(); // lumai
            decimal value = ParseExpression();
            Expect(TokenType.Naidu, "Ожидался разделитель 'naidu!'");
            AssignVariable(ident.Lexeme, value);
        }
        else
        {
            // Expression statement - парсим выражение напрямую, начиная с уже прочитанного идентификатора
            // Возвращаем токен обратно в поток для корректного парсинга
            tokenStream.Rewind();
            decimal value = ParseExpression();
            Expect(TokenType.Naidu, "Ожидался разделитель 'naidu!'");
            
            // Вычисляем выражение (если нужно)
            // Значение уже вычислено при парсинге
        }

        return false;
    }

    /// <summary>
    /// input-statement = "guoleila" , "(", identifier , ")", "naidu!" ;
    /// </summary>
    private bool ParseInput()
    {
        tokenStream.Advance(); // guoleila
        ExpectDelimiter("(");
        Token name = Expect(TokenType.Identifier, "Ожидалось имя переменной для ввода");
        ExpectDelimiter(")");
        Expect(TokenType.Naidu, "Ожидался разделитель 'naidu!'");

        if (environment != null)
        {
            decimal value = environment.ReadNumber();
            AssignVariable(name.Lexeme, value);
        }

        return false;
    }

    /// <summary>
    /// output-statement = "tulalilloo" , "ti" , "amo" , "(" , expression , ")" , "naidu!" ;
    /// </summary>
    private bool ParseOutput()
    {
        tokenStream.Advance(); // tulalilloo
        Expect(TokenType.Ti, "Ожидалось 'ti'");
        Expect(TokenType.Amo, "Ожидалось 'amo'");
        ExpectDelimiter("(");
        decimal value = ParseExpression();
        ExpectDelimiter(")");
        Expect(TokenType.Naidu, "Ожидался разделитель 'naidu!'");

        environment?.WriteNumber(value);
        return false;
    }

    /// <summary>
    /// Разбирает выражение верхнего уровня.
    /// Правила:
    ///     expression = logical-or-expression ;
    /// </summary>
    private decimal ParseExpression()
    {
        return ParseLogicalOrExpression();
    }

    /// <summary>
    /// Разбирает логическое ИЛИ выражение.
    /// Правила:
    ///     logical-or-expression = logical-and-expression , { "bo-ca" , logical-and-expression } ;
    /// </summary>
    private decimal ParseLogicalOrExpression()
    {
        decimal left = ParseLogicalAndExpression();

        while (IsOperator("bo-ca"))
        {
            tokenStream.Advance();
            decimal right = ParseLogicalAndExpression();
            left = (left != 0 || right != 0) ? 1 : 0;
        }

        return left;
    }

    /// <summary>
    /// Разбирает логическое И выражение.
    /// Правила:
    ///     logical-and-expression = logical-not-expression , { "tropa" , logical-not-expression } ;
    /// </summary>
    private decimal ParseLogicalAndExpression()
    {
        decimal left = ParseLogicalNotExpression();

        while (IsOperator("tropa"))
        {
            tokenStream.Advance();
            decimal right = ParseLogicalNotExpression();
            left = (left != 0 && right != 0) ? 1 : 0;
        }

        return left;
    }

    /// <summary>
    /// Разбирает логическое НЕ выражение.
    /// Правила:
    ///     logical-not-expression = equality-expression
    ///                             | "makoroni" , logical-not-expression ;
    /// </summary>
    private decimal ParseLogicalNotExpression()
    {
        if (IsOperator("makoroni"))
        {
            tokenStream.Advance();
            decimal value = ParseLogicalNotExpression();
            return value == 0 ? 1 : 0;
        }

        return ParseEqualityExpression();
    }

    /// <summary>
    /// Разбирает выражение равенства и неравенства.
    /// Правила:
    ///     equality-expression = relational-expression , { ("con" | "nocon") , relational-expression } ;
    /// </summary>
    private decimal ParseEqualityExpression()
    {
        decimal left = ParseRelationalExpression();

        while (IsOperator("con") || IsOperator("nocon"))
        {
            string op = tokenStream.Peek().Lexeme;
            tokenStream.Advance();
            decimal right = ParseRelationalExpression();

            bool result = op == "con" ? left == right : left != right;
            left = result ? 1 : 0;
        }

        return left;
    }

    /// <summary>
    /// Разбирает выражение сравнения.
    /// Правила:
    ///     relational-expression = additive-expression ,
    ///                             { ("looka too" , [ "con" ] | "la" , [ "con" ]) , additive-expression } ;
    /// </summary>
    private decimal ParseRelationalExpression()
    {
        decimal left = ParseAdditiveExpression();

        while (IsOperator("la") || IsOperator("looka"))
        {
            string op = tokenStream.Peek().Lexeme;
            tokenStream.Advance();

            bool hasCon = false;

            // Обработка "la" с опциональным "con"
            if (op == "la")
            {
                if (IsOperator("con"))
                {
                    tokenStream.Advance();
                    hasCon = true;
                }
            }

            // Обработка "looka too" с опциональным "con"
            else if (op == "looka")
            {
                if (!IsOperator("too"))
                {
                    throw new InvalidOperationException("После 'looka' ожидалось 'too'");
                }

                tokenStream.Advance();
                op = "looka too";

                if (IsOperator("con"))
                {
                    tokenStream.Advance();
                    hasCon = true;
                }
            }

            decimal right = ParseAdditiveExpression();

            bool result = op switch
            {
                "la" when !hasCon => left < right,
                "la" when hasCon => left <= right,
                "looka too" when !hasCon => left > right,
                "looka too" when hasCon => left >= right,
                _ => throw new InvalidOperationException($"Неизвестный оператор сравнения: {op}")
            };

            left = result ? 1 : 0;
        }

        return left;
    }

    /// <summary>
    /// Разбирает выражение сложения и вычитания.
    /// Правила:
    ///     additive-expression = multiplicative-expression ,
    ///                           { ("melomo" | "flavuk") , multiplicative-expression } ;
    /// </summary>
    private decimal ParseAdditiveExpression()
    {
        decimal left = ParseMultiplicativeExpression();

        while (IsOperator("melomo") || IsOperator("flavuk"))
        {
            string op = tokenStream.Peek().Lexeme;
            tokenStream.Advance();

            // После бинарного оператора может быть унарный оператор или multiplicative expression
            // Проверяем только на два бинарных оператора подряд (не унарных)
            // Унарные операторы обрабатываются в ParseUnaryExpression
            decimal right = ParseMultiplicativeExpression();

            left = op == "melomo" ? left + right : left - right;
        }

        return left;
    }

    /// <summary>
    /// Разбирает выражение умножения, деления и остатка.
    /// Правила:
    ///     multiplicative-expression = unary-expression ,
    ///                                 { ("dibotada" | "poopaye" | "pado") , unary-expression } ;
    /// </summary>
    private decimal ParseMultiplicativeExpression()
    {
        decimal left = ParseUnaryExpression();

        while (IsOperator("dibotada") || IsOperator("poopaye") || IsOperator("pado"))
        {
            string op = tokenStream.Peek().Lexeme;
            tokenStream.Advance();
            decimal right = ParseUnaryExpression();

            left = op switch
            {
                "dibotada" => left * right,
                "poopaye" => right != 0 ? left / right : throw new DivideByZeroException("Деление на ноль"),
                "pado" => right != 0 ? left % right : throw new DivideByZeroException("Остаток от деления на ноль"),
                _ => throw new InvalidOperationException($"Неизвестный оператор: {op}")
            };
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
    private decimal ParseUnaryExpression()
    {
        if (IsOperator("melomo"))
        {
            tokenStream.Advance();
            return ParseUnaryExpression();
        }

        if (IsOperator("flavuk"))
        {
            tokenStream.Advance();
            return -ParseUnaryExpression();
        }

        return ParsePowerExpression();
    }

    /// <summary>
    /// Разбирает выражение возведения в степень.
    /// Правила:
    ///     power-expression = primary-expression , [ "beedo" , power-expression ] ;
    /// </summary>
    private decimal ParsePowerExpression()
    {
        decimal left = ParsePrimaryExpression();

        if (IsOperator("beedo"))
        {
            tokenStream.Advance();
            decimal right = ParsePowerExpression();
            return (decimal)Math.Pow((double)left, (double)right);
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
    private decimal ParsePrimaryExpression()
    {
        Token token = tokenStream.Peek();

        // Числовой литерал
        if (token.Type == TokenType.NumberLiteral)
        {
            tokenStream.Advance();
            return decimal.Parse(token.Lexeme, CultureInfo.InvariantCulture);
        }

        // Логические литералы
        if (token.Type == TokenType.Da)
        {
            tokenStream.Advance();
            return 1;
        }

        if (token.Type == TokenType.No)
        {
            tokenStream.Advance();
            return 0;
        }

        // Строковые литералы (пока не поддерживаем вычисление)
        if (token.Type == TokenType.StringLiteral)
        {
            tokenStream.Advance();
            throw new NotImplementedException("Вычисление строковых литералов не реализовано");
        }

        // Идентификатор или константа
        if (token.Type == TokenType.Identifier)
        {
            tokenStream.Advance();
            string name = token.Lexeme;

            // Проверяем, является ли это константой
            if (TryGetConstantValue(name, out decimal constValue))
            {
                return constValue;
            }

            // Проверяем, является ли это вызовом функции
            if (IsDelimiter("("))
            {
                return ParseFunctionCall(name);
            }

            // Иначе это переменная
            if (context.TryGetVariable(name, out decimal value))
            {
                return value;
            }

            throw new InvalidOperationException($"Неизвестная переменная: {name}");
        }

        // Скобки
        if (IsDelimiter("("))
        {
            tokenStream.Advance();
            decimal result = ParseExpression();
            ExpectDelimiter(")");
            return result;
        }

        throw new InvalidOperationException($"Неожиданный токен: {token}");
    }

    /// <summary>
    /// Разбирает вызов функции.
    /// Правила:
    ///     function-call = identifier , "(" , [ argument-list ] , ")" ;
    ///     argument-list = expression , { "," , expression } ;
    /// </summary>
    private decimal ParseFunctionCall(string functionName)
    {
        // Уже прочитали идентификатор, ожидаем открывающую скобку
        ExpectDelimiter("(");

        List<decimal> arguments = new List<decimal>();

        // Если сразу закрывающая скобка, то аргументов нет
        if (IsDelimiter(")"))
        {
            tokenStream.Advance();
            return BuiltinFunctions.Invoke(functionName, arguments);
        }

        // Читаем первый аргумент
        arguments.Add(ParseExpression());

        // Читаем остальные аргументы
        while (IsDelimiter(","))
        {
            tokenStream.Advance();
            arguments.Add(ParseExpression());
        }

        // Закрывающая скобка
        ExpectDelimiter(")");
        return BuiltinFunctions.Invoke(functionName, arguments);
    }

    /// <summary>
    /// Проверяет, является ли текущий токен оператором с заданным именем.
    /// </summary>
    private bool IsOperator(string operatorName)
    {
        Token token = tokenStream.Peek();
        return token.Type == TokenType.Operator && token.Lexeme == operatorName;
    }

    private void DefineVariable(string name)
    {
        if (environment != null && !context.TryDefineVariable(name, 0m))
        {
            throw new InvalidOperationException($"Переменная '{name}' уже объявлена в этой области видимости");
        }
    }

    private void AssignVariable(string name, decimal value)
    {
        if (environment != null && !context.TryAssignVariable(name, value))
        {
            throw new InvalidOperationException($"Переменная '{name}' не объявлена");
        }
    }

    private Token Expect(TokenType type, string message)
    {
        Token token = tokenStream.Peek();
        if (token.Type != type)
        {
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
            throw new InvalidOperationException($"Ожидался разделитель '{lexeme}'");
        }

        tokenStream.Advance();
    }

    /// <summary>
    /// Проверяет, является ли текущий токен разделителем с заданным значением.
    /// </summary>
    private bool IsDelimiter(string lexeme)
    {
        Token token = tokenStream.Peek();
        return token.Type == TokenType.Delimiter && token.Lexeme == lexeme;
    }

    /// <summary>
    /// Пытается получить значение константы по идентификатору.
    /// </summary>
    /// <param name="identifier">Идентификатор константы.</param>
    /// <param name="value">Значение константы, если она найдена.</param>
    /// <returns>true, если идентификатор является константой; иначе false.</returns>
    private bool TryGetConstantValue(string identifier, out decimal value)
    {
        if (identifier.Equals("belloPi", StringComparison.OrdinalIgnoreCase))
        {
            value = 3.141592653589793m;
            return true;
        }

        if (identifier.Equals("belloE", StringComparison.OrdinalIgnoreCase))
        {
            value = 2.718281828459045m;
            return true;
        }

        value = 0;
        return false;
    }
}