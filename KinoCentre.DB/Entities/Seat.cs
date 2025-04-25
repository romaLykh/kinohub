namespace KinoCentre.DB.Entities;

public class Seat
{
    public Seat()
    {
    }
    public Seat(int row, int number)
    {
        Row = row;
        Number = number;
    }
    public int Row { get; set; }
    public int Number { get; set; }
    
    public override string ToString()
    {
        return $"Row: {Row}, Number: {Number}";
    }
}