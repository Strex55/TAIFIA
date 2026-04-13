using Lexer;

namespace Parser;

/// <summary>
/// Поток токенов для синтаксического анализа.
/// Предоставляет интерфейс для последовательного чтения токенов.
/// </summary>
public class TokenStream
{
    private readonly Lexer.Lexer lexer;
    private readonly IReadOnlyList<Token> tokens;
    private int currentIndex;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="TokenStream"/>.
    /// </summary>
    /// <param name="code">Исходный код для токенизации.</param>
    public TokenStream(string code)
    {
        lexer = new Lexer.Lexer(code);
        tokens = lexer.Tokenize();
        currentIndex = 0;
    }

    /// <summary>
    /// Возвращает текущий токен без продвижения по потоку.
    /// </summary>
    /// <returns>Текущий токен или токен EndOfFile, если поток закончился.</returns>
    public Token Peek()
    {
        if (currentIndex >= tokens.Count)
        {
            return new Token(TokenType.EndOfFile, "", currentIndex);
        }

        // Пропускаем пробелы и комментарии
        while (currentIndex < tokens.Count)
        {
            Token token = tokens[currentIndex];
            if (token.Type != TokenType.Whitespace && token.Type != TokenType.Comment)
            {
                return token;
            }

            currentIndex++;
        }

        return new Token(TokenType.EndOfFile, "", currentIndex);
    }

    /// <summary>
    /// Перемещает указатель на следующий токен в потоке.
    /// </summary>
    public void Advance()
    {
        if (currentIndex < tokens.Count)
        {
            currentIndex++;
        }
    }

    /// <summary>
    /// Возвращает следующий токен без продвижения по потоку.
    /// </summary>
    /// <returns>Следующий токен или токен EndOfFile, если поток закончился.</returns>
    public Token PeekNext()
    {
        int savedIndex = currentIndex;
        Advance();
        Token next = Peek();
        currentIndex = savedIndex;
        return next;
    }

    /// <summary>
    /// Возвращает указатель на один токен назад (если возможно).
    /// </summary>
    public void Rewind()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
        }
    }
}