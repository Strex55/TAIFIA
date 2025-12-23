namespace SquareRootApp
{
    public class SquareRoot
    {
        public static double? Calculate(double number)
        {
            if (number < 0)
                return null;

            return Math.Sqrt(number);
        }
    }
}
