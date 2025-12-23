using System;
using System.Numerics;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Введите целое неотрицательное число: ");
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int n))
        {
            Console.WriteLine("Некорректный ввод: требуется целое число.");
            return;
        }

        try
        {
            BigInteger result = Factorial.Calculate(n);
            Console.WriteLine($"Факториал {n} = {result}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
