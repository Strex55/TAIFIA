using System;
using System.Collections.Generic;
using Lexer;

namespace Parser
{
    /*
    Главный класс синтаксического анализатора.
    Выполняет разбор и вычисление выражений языка Astra.
    Грамматика языка описана в файле `docs/specification/expressions-grammar.md`.
    */
    public class Parser
    {
        private readonly TokenStream _tokenStream;
        private readonly BuiltinFunctions _builtinFunctions = new BuiltinFunctions();

        public Parser(string code)
        {
            _tokenStream = new TokenStream(code);
        }

        public static int EvaluateExpression(string code)
        {
            Parser parser = new Parser(code);
            decimal result = parser.ParseExpression();
            return (int)Math.Round(result);
        }

        /*
        Правила:
            expression = additive_expression ;
        */
        private decimal ParseExpression()
        {
            return ParseAdditiveExpression();
        }

        /*
        Правила:
            additive_expression = multiplicative_expression, { ("+" | "-"), multiplicative_expression } ;
        */
        private decimal ParseAdditiveExpression()
        {
            decimal result = ParseMultiplicativeExpression();

            while (true)
            {
                if (_tokenStream.TryConsume(TokenType.Plus))
                {
                    decimal right = ParseMultiplicativeExpression();
                    result += right;
                }
                else if (_tokenStream.TryConsume(TokenType.Minus))
                {
                    decimal right = ParseMultiplicativeExpression();
                    result -= right;
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        /*
        Правила:
            multiplicative_expression = power_expression, { ("*" | "/" | "%"), power_expression } ;
        */
        private decimal ParseMultiplicativeExpression()
        {
            decimal result = ParsePowerExpression();

            while (true)
            {
                if (_tokenStream.TryConsume(TokenType.Multiply))
                {
                    decimal right = ParsePowerExpression();
                    result *= right;
                }
                else if (_tokenStream.TryConsume(TokenType.Divide))
                {
                    decimal right = ParsePowerExpression();
                    if (right == 0)
                    {
                        throw new ParserException("Division by zero", _tokenStream.Peek().Position);
                    }
                    result /= right;
                }
                else if (_tokenStream.TryConsume(TokenType.Modulo))
                {
                    decimal right = ParsePowerExpression();
                    if (right == 0)
                    {
                        throw new ParserException("Modulo by zero", _tokenStream.Peek().Position);
                    }
                    result %= right;
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        /*
        Правила:
            power_expression = unary_expression, [ "**", power_expression ] ;
        */
        private decimal ParsePowerExpression()
        {
            decimal result = ParseUnaryExpression();

            if (_tokenStream.TryConsume(TokenType.Power))
            {
                decimal exponent = ParsePowerExpression();
                result = (decimal)Math.Pow((double)result, (double)exponent);
            }

            return result;
        }

        /*
        Правила:
            unary_expression = [ "+" | "-" ], primary_expression ;
        */
        private decimal ParseUnaryExpression()
        {
            bool isNegative = false;

            while (true)
            {
                if (_tokenStream.TryConsume(TokenType.Plus))
                {
                    // Унарный плюс - ничего не меняет
                }
                else if (_tokenStream.TryConsume(TokenType.Minus))
                {
                    isNegative = !isNegative;
                }
                else
                {
                    break;
                }
            }

            decimal result = ParsePrimaryExpression();
            return isNegative ? -result : result;
        }

        /*
        Правила:
            primary_expression = number | constant | function_call | "(", expression, ")" ;
        */
        private decimal ParsePrimaryExpression()
        {
            Token current = _tokenStream.Peek();

            if (current.Type == TokenType.Number)
            {
                return ParseNumber();
            }
            else if (current.Type == TokenType.Identifier)
            {
                if (current.Value == "Pi" || current.Value == "Euler")
                {
                    return ParseConstant();
                }
                else
                {
                    string functionName = current.Value;
                    _tokenStream.Advance();
                    return ParseFunctionCall(functionName);
                }
            }
            else if (_tokenStream.TryConsume(TokenType.LeftParen))
            {
                decimal result = ParseExpression();
                _tokenStream.Consume(TokenType.RightParen, "Expected ')'");
                return result;
            }
            else
            {
                throw new ParserException($"Unexpected token: {current.Value}", current.Position);
            }
        }

        private decimal ParseNumber()
        {
            Token token = _tokenStream.Peek();
            _tokenStream.Advance();

            if (decimal.TryParse(token.Value, System.Globalization.NumberStyles.Any, 
                System.Globalization.CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }

            throw new ParserException($"Invalid number format: {token.Value}", token.Position);
        }

        private decimal ParseConstant()
        {
            Token token = _tokenStream.Peek();
            _tokenStream.Advance();

            if (token.Value == "Pi")
            {
                return (decimal)Math.PI;
            }
            else if (token.Value == "Euler")
            {
                return (decimal)Math.E;
            }
            else
            {
                throw new ParserException($"Unknown constant: {token.Value}", token.Position);
            }
        }

        /*
        Правила:
            function_call = identifier, "(", [ argument_list ], ")" ;
        */
        private decimal ParseFunctionCall(string functionName)
        {
            _tokenStream.Consume(TokenType.LeftParen, "Expected '(' after function name");

            List<decimal> arguments = new List<decimal>();

            if (!_tokenStream.Match(TokenType.RightParen))
            {
                arguments = ParseArgumentList();
            }

            _tokenStream.Consume(TokenType.RightParen, "Expected ')' after function arguments");

            return _builtinFunctions.Invoke(functionName, arguments);
        }

        /*
        Правила:
            argument_list = expression, { ",", expression } ;
        */
        private List<decimal> ParseArgumentList()
        {
            List<decimal> arguments = new List<decimal>();

            arguments.Add(ParseExpression());

            while (_tokenStream.TryConsume(TokenType.Comma))
            {
                arguments.Add(ParseExpression());
            }

            return arguments;
        }
    }
}