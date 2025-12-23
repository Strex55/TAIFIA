using SquareRootApp;
using Xunit;

namespace SquareRootTests
{
    public class SquareRootLogicTests
    {
        [Fact]
        public void PositiveNumber_ReturnsCorrectSqrt()
        {
            double? result = SquareRoot.Calculate(9);

            Assert.NotNull(result);
            Assert.Equal(3.0, result.Value, 10);
        }

        [Fact]
        public void Zero_ReturnsZero()
        {
            double? result = SquareRoot.Calculate(0);

            Assert.NotNull(result);
            Assert.Equal(0.0, result.Value, 10);
        }

        [Fact]
        public void NegativeNumber_ReturnsNull()
        {
            double? result = SquareRoot.Calculate(-5);

            Assert.Null(result);
        }

        [Fact]
        public void Fraction_ReturnsCorrectSqrt()
        {
            double? result = SquareRoot.Calculate(2.25);

            Assert.NotNull(result);
            Assert.Equal(Math.Sqrt(2.25), result.Value, 10);
        }
    }
}