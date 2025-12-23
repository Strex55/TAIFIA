using System;
public static class StringReverser
{
    public static string Reverse(string input)
    {
        if (input == null)
            return "";

        char[] chars = input.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }
}
