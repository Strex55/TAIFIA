using System;
using System.Numerics;
using Xunit;

public class FactorialTests
{
    [Fact]
    public void Factorial_Of_0_Is_1()
    {
        Assert.Equal(BigInteger.One, Factorial.Calculate(0));
    }

    [Fact]
    public void Factorial_Of_1_Is_1()
    {
        Assert.Equal(BigInteger.One, Factorial.Calculate(1));
    }

    [Fact]
    public void Factorial_Of_5_Is_120()
    {
        Assert.Equal(new BigInteger(120), Factorial.Calculate(5));
    }

    [Fact]
    public void Factorial_Of_20_Correct()
    {
        var expected = BigInteger.Parse("2432902008176640000");
        Assert.Equal(expected, Factorial.Calculate(20));
    }

    [Fact]
    public void Factorial_Negative_Throws()
    {
        Assert.Throws<ArgumentException>(() => Factorial.Calculate(-3));
    }
}
