using Sokoban.Models;
using Sokoban.Logging;

namespace Sokoban.Models;

public class LevelStatus(LevelData levelData)
{
    public string Name { get; }= levelData.Name;
    public CellType[,] Grid { get; private set; } = (CellType[,])levelData.Grid.Clone();
    public IReadOnlyList<CellPos> Targets { get; } = new List<CellPos>(levelData.TargetList);
    public int Width => Grid.GetLength(0);
    public int Height => Grid.GetLength(1);
    public bool IsCurBoxInTarget = false;

    public CellType? GetCell(CellPos pos) => !IsInBounds(pos) ? null : Grid[pos.X, pos.Y];

    public bool IsBox(CellPos pos) => GetCell(pos) == CellType.Box || GetCell(pos) == CellType.BoxInTarget;

    public bool IsFreeCell(CellPos pos) => GetCell(pos) == CellType.Empty || GetCell(pos) == CellType.Target;

    public CellType? GetNextCell(CellPos box, Move direction)
    {
        var to = box.Move(direction);
        return !IsInBounds(to) ? null : GetCell(to);
    }

    public bool IsInBounds(CellPos pos) =>
        pos.X >= 0 && pos.X < Width && pos.Y >= 0 && pos.Y < Height;


    public bool MoveBox(CellPos box, Move direction)
    {
        if (!IsBox(box))
        {
            Log.Error($"Function MoveBox is trying to move {GetCell(box)}");
            return false;
        }
        var to = box.Move(direction);

        if (!IsFreeCell(to))
            return false;

        var curCell = GetCell(box);
        var nextCell = GetNextCell(box, direction);

        Grid[box.X, box.Y] = curCell == CellType.BoxInTarget
        ? CellType.Target : CellType.Empty;

        var newCell = nextCell == CellType.Target ? CellType.BoxInTarget : CellType.Box;
        
        Grid[to.X, to.Y] = newCell;

        IsCurBoxInTarget = newCell == CellType.BoxInTarget;
    
        return true;
    }

    public bool IsCompleted()
    {
        return Targets.All(t => Grid[t.X, t.Y] == CellType.BoxInTarget);
    }
}
