using System;
using System.Collections.Generic;

namespace Parser
{
    /*
    Класс для работы со встроенными функциями языка Astra.
    Обеспечивает вызов математических и тригонометрических функций.
    */
    public class BuiltinFunctions
    {
        public decimal Invoke(string name, List<decimal> arguments)
        {
            string lowerName = name.ToLower();

            switch (lowerName)
            {
                case "abs":
                    return Abs(arguments);
                case "min":
                    return Min(arguments);
                case "max":
                    return Max(arguments);
                case "sin":
                    return Sin(arguments);
                case "cos":
                    return Cos(arguments);
                case "tan":
                    return Tan(arguments);
                default:
                    throw new InvalidOperationException($"Unknown function: {name}");
            }
        }

        private decimal Abs(List<decimal> arguments)
        {
            ValidateArgumentCount(nameof(Abs), 1, arguments.Count);
            return Math.Abs(arguments[0]);
        }

        private decimal Min(List<decimal> arguments)
        {
            ValidateArgumentCount(nameof(Min), 1, arguments.Count, true);
            decimal min = arguments[0];
            for (int i = 1; i < arguments.Count; i++)
            {
                if (arguments[i] < min)
                {
                    min = arguments[i];
                }
            }
            return min;
        }

        private decimal Max(List<decimal> arguments)
        {
            ValidateArgumentCount(nameof(Max), 1, arguments.Count, true);
            decimal max = arguments[0];
            for (int i = 1; i < arguments.Count; i++)
            {
                if (arguments[i] > max)
                {
                    max = arguments[i];
                }
            }
            return max;
        }

        private decimal Sin(List<decimal> arguments)
        {
            ValidateArgumentCount(nameof(Sin), 1, arguments.Count);
            return (decimal)Math.Sin((double)arguments[0]);
        }

        private decimal Cos(List<decimal> arguments)
        {
            ValidateArgumentCount(nameof(Cos), 1, arguments.Count);
            return (decimal)Math.Cos((double)arguments[0]);
        }

        private decimal Tan(List<decimal> arguments)
        {
            ValidateArgumentCount(nameof(Tan), 1, arguments.Count);
            return (decimal)Math.Tan((double)arguments[0]);
        }

        private void ValidateArgumentCount(string functionName, int minCount, int actualCount, bool allowMore = false)
        {
            if (!allowMore && actualCount != minCount)
            {
                throw new InvalidOperationException(
                    $"Function {functionName} requires exactly {minCount} argument(s), got {actualCount}");
            }

            if (allowMore && actualCount < minCount)
            {
                throw new InvalidOperationException(
                    $"Function {functionName} requires at least {minCount} argument(s), got {actualCount}");
            }
        }
    }
}