namespace Astra.Lexer;

public class Position
{
    public int Line { get; set; } = 1;
    public int Column { get; set; } = 1;
    public int Absolute { get; set; } = 0;

    public Position() { }

    public Position(int line, int column, int absolute)
    {
        Line = line;
        Column = column;
        Absolute = absolute;
    }

    public Position Clone()
    {
        return new Position(Line, Column, Absolute);
    }

    public void Advance(char currentChar)
    {
        Absolute++;
        
        if (currentChar == '\n')
        {
            Line++;
            Column = 1;
        }
        else
        {
            Column++;
        }
    }

    public override string ToString() => $"Line {Line}, Column {Column}";
}