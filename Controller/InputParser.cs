using Avalonia.Input;
using Sokoban.Models;

namespace Sokoban.Controller;

public static class InputParser
{
    public static Move ParseKey(Key key)
    {
        return key switch
        {
            Key.W or Key.Up    => Move.Up,
            Key.S or Key.Down  => Move.Down,
            Key.A or Key.Left  => Move.Left,
            Key.D or Key.Right => Move.Right,
            _ => Move.None
        };
    }
    
    public static Move ParseString(string input)
    {
        if (string.IsNullOrEmpty(input)) return Move.None;
        
        return input.ToUpper() switch
        {
            "W" or "Ц" or "UP" => Move.Up,
            "S" or "Ы" or "DOWN" => Move.Down,
            "A" or "Ф" or "LEFT" => Move.Left,
            "D" or "В" or "RIGHT" => Move.Right,
            _ => Move.None
        };
    }

    public static CellType CharToCellType(char c)
    {
        return c switch
        {
            '#' => CellType.Wall,
            '*' => CellType.Box,
            '.' => CellType.Target,
            '@' => CellType.Player,
            _ => CellType.Empty
        };
    }
}