using System;

using Xunit;

namespace Parser.UnitTests
{
    public class ParserTests
    {
        [Fact]
        public void Parse_SingleInteger_ReturnsCorrectValue()
        {
            string code = "42";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(42, result);
        }

        [Fact]
        public void Parse_SingleFloat_ReturnsCorrectValue()
        {
            string code = "3.14";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(3, result);
        }

        [Fact]
        public void Parse_SimpleAddition_ReturnsCorrectValue()
        {
            string code = "2 + 3";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Parse_AdditionAndSubtraction_ReturnsCorrectValue()
        {
            string code = "10 + 5 - 3";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(12, result);
        }

        [Fact]
        public void Parse_MultiplicationBeforeAddition_ReturnsCorrectValue()
        {
            string code = "2 + 3 * 4";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(14, result);
        }

        [Fact]
        public void Parse_ParenthesesChangePriority_ReturnsCorrectValue()
        {
            string code = "(2 + 3) * 4";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(20, result);
        }

        [Fact]
        public void Parse_UnaryMinus_ReturnsCorrectValue()
        {
            string code = "-5";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(-5, result);
        }

        [Fact]
        public void Parse_DoubleUnaryMinus_ReturnsCorrectValue()
        {
            string code = "--5";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Parse_PowerOperation_ReturnsCorrectValue()
        {
            string code = "2 ** 3";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(8, result);
        }

        [Fact]
        public void Parse_PowerWithRightAssociativity_ReturnsCorrectValue()
        {
            string code = "2 ** 3 ** 2";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(512, result);
        }

        [Fact]
        public void Parse_ConstantPi_ReturnsCorrectValue()
        {
            string code = "Pi";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(3, result);
        }

        [Fact]
        public void Parse_ConstantEuler_ReturnsCorrectValue()
        {
            string code = "Euler";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(3, result);
        }

        [Fact]
        public void Parse_FunctionCallSin_ReturnsCorrectValue()
        {
            string code = "sin(0)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(0, result);
        }

        [Fact]
        public void Parse_FunctionCallAbs_ReturnsCorrectValue()
        {
            string code = "abs(-10)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(10, result);
        }

        [Fact]
        public void Parse_FunctionCallMin_ReturnsCorrectValue()
        {
            string code = "min(5, 3, 8, 2)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(2, result);
        }

        [Fact]
        public void Parse_FunctionCallMax_ReturnsCorrectValue()
        {
            string code = "max(5, 3, 8, 2)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(8, result);
        }

        [Fact]
        public void Parse_ComplexExpression_ReturnsCorrectValue()
        {
            string code = "2 * sin(Pi/2) + cos(0)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(3, result);
        }

        [Fact]
        public void Parse_Division_ReturnsCorrectValue()
        {
            string code = "10 / 2";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Parse_Modulo_ReturnsCorrectValue()
        {
            string code = "10 % 3";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(1, result);
        }

        [Fact]
        public void Parse_MultipleOperations_ReturnsCorrectValue()
        {
            string code = "2 + 3 * 4 - 8 / 2";
            int result = Parser.EvaluateExpression(code);
            // Правильный расчет: 2 + (3*4) - (8/2) = 2 + 12 - 4 = 10
            Assert.Equal(10, result);
        }

        [Fact]
        public void Parse_NestedFunctions_ReturnsCorrectValue()
        {
            string code = "max(min(5, 10), 3)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Parse_ExpressionWithSpaces_ReturnsCorrectValue()
        {
            string code = "   2   +   3   *   4   ";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(14, result);
        }

        [Fact]
        public void Parse_EmptyParentheses_ThrowsException()
        {
            string code = "sin()";
            // Функция sin() без аргументов бросает InvalidOperationException
            Assert.Throws<InvalidOperationException>(() => Parser.EvaluateExpression(code));
        }

        [Fact]
        public void Parse_MissingParenthesis_ThrowsException()
        {
            string code = "(2 + 3";
            Assert.Throws<global::Parser.ParserException>(() => Parser.EvaluateExpression(code));
        }

        [Fact]
        public void Parse_UnknownFunction_ThrowsException()
        {
            string code = "unknown(5)";
            // Неизвестная функция бросает InvalidOperationException
            Assert.Throws<InvalidOperationException>(() => Parser.EvaluateExpression(code));
        }

        [Fact]
        public void Parse_DivisionByZero_ThrowsException()
        {
            string code = "5 / 0";
            Assert.Throws<global::Parser.ParserException>(() => Parser.EvaluateExpression(code));
        }

        // Добавим дополнительные тесты для проверки

        [Fact]
        public void Parse_FunctionCallWithExpression_ReturnsCorrectValue()
        {
            string code = "sin(Pi/2)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(1, result);
        }

        [Fact]
        public void Parse_FunctionCallCos_ReturnsCorrectValue()
        {
            string code = "cos(0)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(1, result);
        }

        [Fact]
        public void Parse_ExpressionWithAllOperators_ReturnsCorrectValue()
        {
            string code = "1 + 2 * 3 - 4 / 2 + 2 ** 3";
            // 1 + 6 - 2 + 8 = 13
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(13, result);
        }

        [Fact]
        public void Parse_Zero_ReturnsCorrectValue()
        {
            string code = "0";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(0, result);
        }

        [Fact]
        public void Parse_LargeInteger_ReturnsCorrectValue()
        {
            string code = "1234567890";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(1234567890, result);
        }

        [Fact]
        public void Parse_Hundred_ReturnsCorrectValue()
        {
            string code = "100";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(100, result);
        }

        [Fact]
        public void Parse_ZeroFloat_ReturnsCorrectValue()
        {
            string code = "0.0";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(0, result);
        }

        [Fact]
        public void Parse_SmallFloat_ReturnsCorrectValue()
        {
            string code = "0.001";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(0, result);
        }

        [Fact]
        public void Parse_LongFloat_ReturnsCorrectValue()
        {
            string code = "123.456";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(123, result);
        }

        [Fact]
        public void Parse_FloatWithLeadingZero_ReturnsCorrectValue()
        {
            string code = "0.5";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(0, result);
        }

        [Fact]
        public void Parse_UnaryPlusBeforeFloat_ReturnsCorrectValue()
        {
            string code = "+3.14";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(3, result);
        }

        [Fact]
        public void Parse_UnaryPlusBeforeParentheses_ReturnsCorrectValue()
        {
            string code = "+(2 + 3)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Parse_UnaryMinusBeforeFloat_ReturnsCorrectValue()
        {
            string code = "-3.14";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(-3, result);
        }

        [Fact]
        public void Parse_UnaryMinusBeforeParentheses_ReturnsCorrectValue()
        {
            string code = "-(2 + 3)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(-5, result);
        }

        [Fact]
        public void Parse_ChainAddition_ReturnsCorrectValue()
        {
            string code = "1 + 2 + 3 + 4";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(10, result);
        }

        [Fact]
        public void Parse_ChainSubtraction_ReturnsCorrectValue()
        {
            string code = "10 - 2 - 3";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Parse_MixedAdditionSubtractionChain_ReturnsCorrectValue()
        {
            string code = "10 + 5 - 3 + 2";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(14, result);
        }

        [Fact]
        public void Parse_LeftAssociativitySubtraction_ReturnsCorrectValue()
        {
            string code = "10 - 5 - 2";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(3, result); // (10 - 5) - 2 = 3
        }

        [Fact]
        public void Parse_FloatAddition_ReturnsCorrectValue()
        {
            string code = "1.5 + 2.5";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(4, result);
        }

        [Fact]
        public void Parse_FloatSubtraction_ReturnsCorrectValue()
        {
            string code = "5.5 - 2.2";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(3, result);
        }

        [Fact]
        public void Parse_Multiplication_ReturnsCorrectValue()
        {
            string code = "2 * 3";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(6, result);
        }

        [Fact]
        public void Parse_MultiplicationChain_ReturnsCorrectValue()
        {
            string code = "2 * 3 * 4";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(24, result);
        }

        [Fact]
        public void Parse_MixedMultiplicationDivision_ReturnsCorrectValue()
        {
            string code = "10 * 2 / 5";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(4, result);
        }

        [Fact]
        public void Parse_LeftAssociativityDivision_ReturnsCorrectValue()
        {
            string code = "12 / 3 / 2";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(2, result); // (12 / 3) / 2 = 2
        }

        [Fact]
        public void Parse_FloatMultiplication_ReturnsCorrectValue()
        {
            string code = "2.5 * 4.0";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(10, result);
        }

        [Fact]
        public void Parse_FloatDivision_ReturnsCorrectValue()
        {
            string code = "5.0 / 2.0";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(2, result);
        }

        [Fact]
        public void Parse_PowerFloat_ReturnsCorrectValue()
        {
            string code = "2.5 ** 2";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(6, result); // 2.5^2 = 6.25, округляем до 6
        }

        [Fact]
        public void Parse_DivisionPriorityOverSubtraction_ReturnsCorrectValue()
        {
            string code = "10 - 8 / 2";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(6, result);
        }

        [Fact]
        public void Parse_PowerPriorityOverMultiplication_ReturnsCorrectValue()
        {
            string code = "2 * 3 ** 2";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(18, result);
        }

        [Fact]
        public void Parse_ComplexExpression1_ReturnsCorrectValue()
        {
            string code = "1 + 2 * 3 ** 2 - 4 / 2";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(17, result); // 1 + 2*9 - 2 = 1 + 18 - 2 = 17
        }

        [Fact]
        public void Parse_ParenthesesAroundNumber_ReturnsCorrectValue()
        {
            string code = "(42)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(42, result);
        }

        [Fact]
        public void Parse_NestedParentheses_ReturnsCorrectValue()
        {
            string code = "((2 + 3) * (4 - 1))";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(15, result);
        }

        [Fact]
        public void Parse_DeeplyNestedParentheses_ReturnsCorrectValue()
        {
            string code = "((((42))))";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(42, result);
        }

        [Fact]
        public void Parse_ParenthesesInPower_ReturnsCorrectValue()
        {
            string code = "2 ** (3 + 1)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(16, result);
        }

        [Fact]
        public void Parse_ExpressionWithParentheses1_ReturnsCorrectValue()
        {
            string code = "(1 + 2) * (3 + 4)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(21, result);
        }

        [Fact]
        public void Parse_ExpressionWithParentheses2_ReturnsCorrectValue()
        {
            string code = "(10 - 2) / (3 + 1)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(2, result);
        }

        [Fact]
        public void Parse_ExpressionWithParentheses3_ReturnsCorrectValue()
        {
            string code = "2 * (3 + 4 * (5 - 2))";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(30, result); // 2 * (3 + 4*3) = 2 * 15 = 30
        }

        [Fact]
        public void Parse_PiInExpression_ReturnsCorrectValue()
        {
            string code = "2 * Pi";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(6, result);
        }

        [Fact]
        public void Parse_PiDivision_ReturnsCorrectValue()
        {
            string code = "Pi / 2";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(2, result);
        }

        [Fact]
        public void Parse_PiInParentheses_ReturnsCorrectValue()
        {
            string code = "(Pi)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(3, result);
        }

        [Fact]
        public void Parse_EulerPower_ReturnsCorrectValue()
        {
            string code = "Euler ** 2";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(7, result); // e^2 ≈ 7.389
        }

        [Fact]
        public void Parse_EulerWithOperations_ReturnsCorrectValue()
        {
            string code = "1 + Euler";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(4, result);
        }

        [Fact]
        public void Parse_PiTimesEuler_ReturnsCorrectValue()
        {
            string code = "Pi * Euler";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(9, result); // π*e ≈ 8.539
        }

        [Fact]
        public void Parse_CosFunction_ReturnsCorrectValue()
        {
            string code = "cos(0)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(1, result);
        }

        [Fact]
        public void Parse_TanFunction_ReturnsCorrectValue()
        {
            string code = "tan(0)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(0, result);
        }

        [Fact]
        public void Parse_FunctionWithExpressionArgument_ReturnsCorrectValue()
        {
            string code = "sin(2 * Pi)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(0, result);
        }

        [Fact]
        public void Parse_MinWithExpressionArguments_ReturnsCorrectValue()
        {
            string code = "min(1 + 2, 3 * 4)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(3, result);
        }

        [Fact]
        public void Parse_MaxWithFunctionArguments_ReturnsCorrectValue()
        {
            string code = "max(sin(0), cos(0))";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(1, result);
        }

        [Fact]
        public void Parse_MinPower_ReturnsCorrectValue()
        {
            string code = "min(1 + 2, 3 * 4) ** 2";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(9, result); // min(3, 12)^2 = 3^2 = 9
        }

        [Fact]
        public void Parse_AbsDivMax_ReturnsCorrectValue()
        {
            string code = "abs(-5 * 2) / max(2, 3)";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(3, result); // 10 / 3 = 3.33 → 3
        }

        [Fact]
        public void Parse_LongPi_ReturnsCorrectValue()
        {
            string code = "3.141592653589793";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(3, result);
        }

        [Fact]
        public void Parse_MultipleUnaryOperators_ReturnsCorrectValue()
        {
            string code = "+-+-+-+-5";
            int result = Parser.EvaluateExpression(code);
            Assert.Equal(5, result);
        }
    }
}
