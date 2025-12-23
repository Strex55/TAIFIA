using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Введите строку: ");
        string? input = Console.ReadLine();

        string reversed = StringReverser.Reverse(input ?? "");
        Console.WriteLine($"Перевёрнутая строка: {reversed}");
    }
}
