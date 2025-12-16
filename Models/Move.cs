namespace Sokoban.Models;


public class Move(int dx, int dy)
{
    public static Move Up => new(0, -1);
    public static Move Down => new(0, 1);
    public static Move Left => new(-1, 0);
    public static Move Right => new(1, 0);
    public static Move None => new(0, 0);
    public int Dx { get; } = dx; 
    public int Dy { get; } = dy;

    public override string ToString()
    {
        if (Dx == 0 && Dy == -1) return "Вверх";
        if (Dx == 0 && Dy == 1) return "Вниз";
        if (Dx == -1 && Dy == 0) return "Влево";
        if (Dx == 1 && Dy == 0) return "Вправо";
        if (Dx == 0 && Dy == 0) return "Нет движения";
        return $"({Dx}, {Dy})";
    }
}
