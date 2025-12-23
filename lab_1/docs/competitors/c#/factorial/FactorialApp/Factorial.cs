using System;
using System.Numerics;

public static class Factorial
{
    public static BigInteger Calculate(int n)
    {
        if (n < 0)
            throw new ArgumentException("Факториал для отрицательных чисел не определён.", nameof(n));

        BigInteger result = BigInteger.One;

        for (int i = 2; i <= n; i++)
            result *= i;

        return result;
    }
}
