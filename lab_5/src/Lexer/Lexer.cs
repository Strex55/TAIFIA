using System;
using System.Collections.Generic;
using System.Text;

namespace Lexer
{
    public class Lexer
    {
        private readonly string source;
        private readonly List<Token> tokens = new();
        private int position;

        public Lexer(string source)
        {
            this.source = (source ?? string.Empty).Replace("\r\n", "\n").Replace('\r', '\n');
            position = 0;
        }

        public IReadOnlyList<Token> Tokenize()
        {
            while (!IsAtEnd())
            {
                if (char.IsWhiteSpace(Peek()))
                {
                    ReadWhitespace();
                    continue;
                }

                if (IsAtEnd())
                {
                    break;
                }

                char c = Peek();

                if (IsStartOfSingleLineComment())
                {
                    ReadSingleLineComment();
                }
                else if (IsStartOfMultiLineComment())
                {
                    ReadMultiLineComment();
                }
                else if (char.IsLetter(c) || c == '_')
                {
                    ReadWordLike();
                }
                else if (char.IsDigit(c) || (c == '-' && char.IsDigit(Peek(1))))
                {
                    ReadNumber();
                }
                else if (c == '!')
                {
                    ReadBangString();
                }
                else if (c == '"')
                {
                    ReadQuotedString();
                }
                else
                {
                    ReadDelimiterOrSymbol();
                }
            }

            return tokens;
        }

        private void ReadWhitespace()
        {
            int start = position;
            while (!IsAtEnd() && char.IsWhiteSpace(Peek()))
            {
                Advance();
            }

            tokens.Add(new Token(TokenType.Whitespace, source[start..position], start));
        }

        private void ReadWordLike()
        {
            int start = position;
            while (!IsAtEnd() && (char.IsLetterOrDigit(Peek()) || Peek() == '_' || Peek() == '-'))
            {
                Advance();
            }

            string word = source[start..position];

            if (Peek() == '!')
            {
                string withBang = word + "!";
                TokenType? keywordType = GetKeywordTokenType(withBang);
                if (keywordType.HasValue)
                {
                    Advance();
                    tokens.Add(new Token(keywordType.Value, withBang, start));
                    return;
                }
            }

            if (IsOperator(word))
            {
                tokens.Add(new Token(TokenType.Operator, word, start));
                return;
            }

            TokenType? keywordTokenType = GetKeywordTokenType(word);
            if (keywordTokenType.HasValue)
            {
                tokens.Add(new Token(keywordTokenType.Value, word, start));
                return;
            }

            tokens.Add(new Token(TokenType.Identifier, word, start));
        }

        private void ReadNumber()
        {
            int start = position;
            if (Peek() == '-')
            {
                Advance();
            }

            while (!IsAtEnd() && char.IsDigit(Peek()))
            {
                Advance();
            }

            if (!IsAtEnd() && Peek() == '.' && char.IsDigit(Peek(1)))
            {
                Advance();
                while (!IsAtEnd() && char.IsDigit(Peek()))
                {
                    Advance();
                }
            }

            // Проверка на ошибку: если после числа идет буква без пробела, это ошибка
            if (!IsAtEnd() && char.IsLetter(Peek()))
            {
                // Читаем оставшуюся часть как ошибку
                while (!IsAtEnd() && (char.IsLetterOrDigit(Peek()) || Peek() == '_' || Peek() == '-'))
                {
                    Advance();
                }

                string errorWord = source[start..position];
                tokens.Add(new Token(TokenType.Error, errorWord, start));
                return;
            }

            tokens.Add(new Token(TokenType.NumberLiteral, source[start..position], start));
        }

        private void ReadBangString()
        {
            int start = position;
            Advance();

            if (Peek() == ')')
            {
                string single = source.Substring(start, 1);
                tokens.Add(new Token(TokenType.StringLiteral, single, start));
                return;
            }

            while (!IsAtEnd())
            {
                if (Peek() == '!' && Peek(1) == '!')
                {
                    Advance(2);
                    continue;
                }

                if (Peek() == '!')
                {
                    break;
                }

                Advance();
            }

            if (!IsAtEnd())
            {
                Advance();
            }

            string lexeme = source[start..position];
            tokens.Add(new Token(TokenType.StringLiteral, lexeme, start));
        }

        private void ReadQuotedString()
        {
            int start = position;
            StringBuilder sb = new StringBuilder();
            sb.Append('"');
            Advance();

            while (!IsAtEnd())
            {
                if (Peek() == '"')
                {
                    sb.Append('"');
                    Advance();
                    break;
                }

                sb.Append(Peek());
                Advance();
            }

            tokens.Add(new Token(TokenType.StringLiteral, sb.ToString(), start));
        }

        private void ReadSingleLineComment()
        {
            int start = position;
            Advance(2);
            while (!IsAtEnd() && Peek() != '\n')
            {
                Advance();
            }

            tokens.Add(new Token(TokenType.Comment, "//" + source[(start + 2)..position], start));
        }

        private void ReadMultiLineComment()
        {
            int start = position;
            Advance(2);
            while (!IsAtEnd() && !(Peek() == '*' && Peek(1) == '/'))
            {
                Advance();
            }

            if (!IsAtEnd())
            {
                Advance(2);
            }

            string text = source[start..position];
            tokens.Add(new Token(TokenType.Comment, text, start));
        }

        private void ReadDelimiterOrSymbol()
        {
            int pos = position;
            char c = Peek();
            if (c == '(' || c == ')')
            {
                Advance();
                tokens.Add(new Token(TokenType.Delimiter, c.ToString(), pos));
                return;
            }

            int start = position;
            while (!IsAtEnd() && !char.IsLetterOrDigit(Peek()) && !char.IsWhiteSpace(Peek()))
            {
                Advance();
            }

            string sym = source[start..position];
            tokens.Add(new Token(TokenType.Delimiter, sym, start));
        }

        private bool IsStartOfSingleLineComment() => Peek() == '/' && Peek(1) == '/';

        private bool IsStartOfMultiLineComment() => Peek() == '/' && Peek(1) == '*';

        private char Peek(int offset = 0)
        {
            int idx = position + offset;
            return idx < source.Length ? source[idx] : '\0';
        }

        private void Advance(int count = 1)
        {
            position = Math.Min(position + count, source.Length);
        }

        private bool IsAtEnd() => position >= source.Length;

        private static TokenType? GetKeywordTokenType(string word)
        {
            return word switch
            {
                "bello!" => TokenType.Bello,
                "oca!" => TokenType.Oca,
                "stopa" => TokenType.Stopa,
                "bapple" => TokenType.Bapple,
                "poop" => TokenType.Poop,
                "trusela" => TokenType.Trusela,
                "bi-do" => TokenType.BiDo,
                "uh-oh" => TokenType.UhOh,
                "again" => TokenType.Again,
                "kemari" => TokenType.Kemari,
                "aspetta" => TokenType.Aspetta,
                "tulalilloo" => TokenType.Tulalilloo,
                "ti" => TokenType.Ti,
                "amo" => TokenType.Amo,
                "guoleila" => TokenType.Guoleila,
                "tank" => TokenType.Tank,
                "yu" => TokenType.Yu,
                "boss" => TokenType.Boss,
                "boo-ya" => TokenType.BooYa,
                "naidu!" => TokenType.Naidu,
                "loka" => TokenType.Loka,
                "Da" => TokenType.Da,
                "No" => TokenType.No,
                "da" => TokenType.Da,
                "no" => TokenType.No,
                _ => null,
            };
        }

        private static bool IsOperator(string word)
        {
            ReadOnlySpan<string> ops = new[]
            {
                "lumai", "beedo", "dibotada", "poopaye", "pado",
                "melomo", "flavuk", "con", "nocon", "looka", "too", "la", "lacon",
                "makoroni", "tropa", "bo-ca",
            };
            foreach (string o in ops)
            {
                if (string.Equals(o, word, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }
    }
}