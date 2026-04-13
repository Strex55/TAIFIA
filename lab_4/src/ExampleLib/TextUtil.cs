using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleLib;

public static class TextUtil
{
    // Символы Unicode, которые мы принимаем как дефис.
    private static readonly Rune[] Hyphens = [new Rune('‐'), new Rune('-')];

    // Символы Unicode, которые мы принимаем как апостроф.
    private static readonly Rune[] Apostrophes = [new Rune('\''), new Rune('`')];

    // Состояния распознавателя слов.
    private enum WordState
    {
        NoWord,
        Letter,
        Hyphen,
        Apostrophe,
    }

    /// <summary>
    ///  Распознаёт слова в тексте. Поддерживает Unicode, в том числе английский и русский языки.
    ///  Слово состоит из букв, может содержать дефис в середине и апостроф в середине либо в конце.
    /// </summary>
    /// <remarks>
    ///  Функция использует автомат-распознаватель с четырьмя состояниями:
    ///   1. NoWord — автомат находится вне слова;
    ///   2. Letter — автомат находится в буквенной части слова;
    ///   3. Hyphen — автомат обработал дефис;
    ///   4. Apostrophe — автомат обработал апостроф.
    ///
    ///  Переходы между состояниями:
    ///   - NoWord → Letter — при получении буквы;
    ///   - Letter → Hyphen — при получении дефиса;
    ///   - Letter → Apostrophe — при получении апострофа;
    ///   - Letter → NoWord — при получении любого символа, кроме буквы, дефиса или апострофа;
    ///   - Hyphen → Letter — при получении буквы;
    ///   - Hyphen → NoWord — при получении любого символа, кроме буквы;
    ///   - Apostrophe → Letter — при получении буквы;
    ///   - Apostrophe → NoWord — при получении любого символа, кроме буквы.
    ///
    ///  Разница между состояниями Hyphen и Apostrophe в том, что дефис не может стоять в конце слова.
    /// </remarks>
    public static List<string> ExtractWords(string text)
    {
        WordState state = WordState.NoWord;

        List<string> results = [];
        StringBuilder currentWord = new();
        foreach (Rune ch in text.EnumerateRunes())
        {
            switch (state)
            {
                case WordState.NoWord:
                    if (Rune.IsLetter(ch))
                    {
                        PushCurrentWord();
                        currentWord.Append(ch);
                        state = WordState.Letter;
                    }

                    break;

                case WordState.Letter:
                    if (Rune.IsLetter(ch))
                    {
                        currentWord.Append(ch);
                    }
                    else if (Hyphens.Contains(ch))
                    {
                        currentWord.Append(ch);
                        state = WordState.Hyphen;
                    }
                    else if (Apostrophes.Contains(ch))
                    {
                        currentWord.Append(ch);
                        state = WordState.Apostrophe;
                    }
                    else
                    {
                        state = WordState.NoWord;
                    }

                    break;

                case WordState.Hyphen:
                    if (Rune.IsLetter(ch))
                    {
                        currentWord.Append(ch);
                        state = WordState.Letter;
                    }
                    else
                    {
                        // Убираем дефис, которого не должно быть в конце слова.
                        currentWord.Remove(currentWord.Length - 1, 1);
                        state = WordState.NoWord;
                    }

                    break;

                case WordState.Apostrophe:
                    if (Rune.IsLetter(ch))
                    {
                        currentWord.Append(ch);
                        state = WordState.Letter;
                    }
                    else
                    {
                        state = WordState.NoWord;
                    }

                    break;
            }
        }

        PushCurrentWord();

        return results;

        void PushCurrentWord()
        {
            if (currentWord.Length > 0)
            {
                results.Add(currentWord.ToString());
                currentWord.Clear();
            }
        }
    }

    /// <summary>
    /// Преобразует римское число в арабское (0..3000).
    /// Пустая строка = 0. Бросает исключение для некорректного римского числа.
    /// </summary>
    public static int ParseRoman(string text)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        if (text.Length == 0)
        {
            return 0;
        }

        // Значение каждого символа
        static int Val(char c) => c switch
        {
            'I' => 1,
            'V' => 5,
            'X' => 10,
            'L' => 50,
            'C' => 100,
            'D' => 500,
            'M' => 1000,
            _ => -1
        };

        // Проверка допустимой вычитающей пары (a перед b)
        static bool IsValidSubtractive(char a, char b) =>
            (a == 'I' && (b == 'V' || b == 'X')) ||
            (a == 'X' && (b == 'L' || b == 'C')) ||
            (a == 'C' && (b == 'D' || b == 'M'));

        int total = 0;
        char lastSymbol = '\0';
        int repeatCount = 0;

        int i = 0;
        while (i < text.Length)
        {
            char cur = text[i];
            int curVal = Val(cur);
            if (curVal < 0)
            {
                throw new ArgumentException($"Invalid Roman character '{cur}'", nameof(text));
            }

            // Повторение символов
            if (cur == lastSymbol)
            {
                repeatCount++;
            }
            else
            {
                lastSymbol = cur;
                repeatCount = 1;
            }

            // Ограничения на повторения
            if ((cur == 'V' || cur == 'L' || cur == 'D') && repeatCount > 1)
            {
                throw new ArgumentException($"Invalid repetition of '{cur}'", nameof(text));
            }

            if ((cur == 'I' || cur == 'X' || cur == 'C' || cur == 'M') && repeatCount > 3)
            {
                throw new ArgumentException($"Too many repetitions of '{cur}'", nameof(text));
            }

            // Проверяем вычитание с последующим символом
            if (i + 1 < text.Length)
            {
                char next = text[i + 1];
                int nextVal = Val(next);
                if (nextVal < 0)
                {
                    throw new ArgumentException($"Invalid Roman character '{next}'", nameof(text));
                }

                if (curVal < nextVal)
                {
                    if (!IsValidSubtractive(cur, next))
                    {
                        throw new ArgumentException($"Invalid subtractive pair '{cur}{next}'", nameof(text));
                    }

                    if (repeatCount > 1)
                    {
                        throw new ArgumentException($"Invalid repeated symbol before subtractive pair '{cur}{next}'", nameof(text));
                    }

                    total += nextVal - curVal;
                    i += 2;
                    lastSymbol = '\0';
                    repeatCount = 0;
                    continue;
                }
            }

            total += curVal;
            i++;
        }

        if (total < 0 || total > 3000)
        {
            throw new ArgumentOutOfRangeException(nameof(text), "Value out of range 0..3000");
        }

        return total;
    }
}