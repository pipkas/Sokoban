using System.Drawing;
namespace Sokoban.Models;

public class Player(CellPos startPos)
{
    public CellPos Position { get; private set; } = startPos;

    public void MoveTo(CellPos newPosition)
    {
        Position = newPosition;
    }
}