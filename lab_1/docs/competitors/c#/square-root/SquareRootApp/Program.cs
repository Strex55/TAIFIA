using System.Globalization;
using SquareRootApp;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Введите число: ");

        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("ERROR");
            return;
        }

        double number;

        bool parsed =
            double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out number) ||
            double.TryParse(input, NumberStyles.Any, CultureInfo.GetCultureInfo("ru-RU"), out number);

        if (!parsed)
        {
            Console.WriteLine("ERROR");
            return;
        }

        double? result = SquareRoot.Calculate(number);

        if (result == null)
        {
            Console.WriteLine("ERROR");
        }
        else
        {
            Console.WriteLine(result.Value.ToString("G", CultureInfo.InvariantCulture));
        }
    }
}