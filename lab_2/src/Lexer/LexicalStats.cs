using System.Text;

namespace Astra.Lexer;

public static class LexicalStats
{
    public static string CollectFromFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"File not found: {path}");
        }

        string source = File.ReadAllText(path);
        Lexer lexer = new Lexer(source);

        Dictionary<string, int> categories = new Dictionary<string, int>
        {
            ["keywords"] = 0,
            ["identifiers"] = 0,
            ["number literals"] = 0,
            ["string literals"] = 0,
            ["operators"] = 0,
            ["other lexemes"] = 0
        };

        Token token;
        while ((token = lexer.NextToken()).Type != TokenType.EndOfFile)
        {
            if (token.Type == TokenType.Error)
            {
                continue; // Пропускаем ошибки
            }

            string category = GetTokenCategory(token.Type);
            categories[category]++;
        }

        // Формируем строку результатов в определенном порядке
        StringBuilder result = new StringBuilder();
        result.AppendLine($"keywords: {categories["keywords"]}");
        result.AppendLine($"identifiers: {categories["identifiers"]}");
        result.AppendLine($"number literals: {categories["number literals"]}");
        result.AppendLine($"string literals: {categories["string literals"]}");
        result.AppendLine($"operators: {categories["operators"]}");
        result.Append($"other lexemes: {categories["other lexemes"]}");

        return result.ToString();
    }

    private static string GetTokenCategory(TokenType type)
    {
        return type switch
        {
            // ВСЕ ключевые слова 
            TokenType.Start or TokenType.End or TokenType.Namespace or TokenType.Import
            or TokenType.Let or TokenType.Const or TokenType.Func or TokenType.Return
            or TokenType.If or TokenType.Else or TokenType.For or TokenType.In
            or TokenType.While or TokenType.Break or TokenType.Continue or TokenType.Show
            or TokenType.True or TokenType.False or TokenType.Null or TokenType.Type
            or TokenType.And or TokenType.Or or TokenType.Not
                => "keywords",

            // Идентификаторы
            TokenType.Identifier => "identifiers",

            // Числовые литералы
            TokenType.NumberLiteral => "number literals",

            // Строковые литералы
            TokenType.StringLiteral => "string literals",

            // Прочие: ->
            TokenType.Plus or TokenType.Minus or TokenType.Multiply or TokenType.Divide
            or TokenType.Modulo or TokenType.Power
            or TokenType.Equal or TokenType.NotEqual
            or TokenType.Less or TokenType.LessEqual or TokenType.Greater or TokenType.GreaterEqual
            or TokenType.Assign or TokenType.PlusAssign or TokenType.MinusAssign
            or TokenType.MultiplyAssign or TokenType.DivideAssign or TokenType.ModuloAssign
            or TokenType.ReturnType
                => "operators",

            // ВСЁ ОСТАЛЬНОЕ - "other lexemes"
            _ => "other lexemes"
        };
    }
}