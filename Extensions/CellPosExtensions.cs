using Sokoban.Models;

namespace Sokoban;

public static class CellPosExtensions
{
    
    public static CellPos Move(this CellPos CellPos, Move direction)
    {
        return new CellPos(CellPos.X + direction.Dx, CellPos.Y + direction.Dy);
    }

} 
