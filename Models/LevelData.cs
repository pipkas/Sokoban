using System.Drawing;
using Sokoban.Models;
using Sokoban.Controller;
using Sokoban.Logging;

namespace Sokoban.Models;

public class LevelData
{
    public string Name { get; set; }
    public CellPos PlayerPos { get; set; }
    public CellType[,] Grid { get; private set;}
    public List<CellPos> TargetList { get; private set;}
    public int BoxCount { get; set; }
    public int TargetCount { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public bool IsValid { get; set; }

    public LevelData(GameLevel gameLevel)
    {
        Name = gameLevel.Name;
        var layout = gameLevel.Layout;
        Width = layout[0].Length;
        Height = layout.Length;
        Grid = new CellType[Width,Height];
        TargetList = new List<CellPos> ();
        IsValid = ValidateAndParse(layout);
        if (layout == null || layout.Length == 0)
        {
            Log.Warning($"Layout is null or empty in map {Name}");
            IsValid = false;
            return;
        }
    }

    private bool ValidateAndParse(string[] layout)
    {
        var playerCount = 0;
        for (int y = 0; y < Height; y++)
        {
            if (string.IsNullOrEmpty(layout[y]) || layout[y].Length != Width)
            {
                Log.Warning($"In map {Name} wrong length of lines");
                return false;
            }
            for (int x = 0; x < layout[y].Length; x++)
            {
                var cell = InputParser.CharToCellType(layout[y][x]);
                Grid[x, y] = cell;
                switch (cell)
                {
                    case CellType.Player:
                        if (playerCount != 0)
                        {
                            Log.Warning($"You have more than 1 player in map {Name}");
                            return false;
                        }
                        PlayerPos = new CellPos(x, y);
                        Grid[x, y] = CellType.Empty;
                        playerCount++;
                        break;
                    case CellType.Box: BoxCount++; break;
                    case CellType.Target: TargetList.Add(new CellPos(x, y)); break;
                    default: break;
                }
            }
        }
        TargetCount = TargetList.Count;
        if (TargetCount != BoxCount)
        {
            Log.Warning($"Number of targets != number of boxes in map {Name}");
            return false;
        }
        if (playerCount == 0)
        {
            Log.Warning($"There is no player in map {Name}");
            return false;
        }
        if (!AreTargetsConnected())
        {
            Log.Warning($"Targets are not connected in map {Name}");
            return false;
        }
        return true;
    }

    private bool AreTargetsConnected()
    {
        if (TargetList.Count == 0)
            return false;

        var visited = new bool[Width, Height];
        var queue = new Queue<CellPos>();

        var start = TargetList[0];
        queue.Enqueue(start);
        visited[start.X, start.Y] = true;

        int connectedTargets = 1;

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            for (int i = 0; i < dx.Length; i++)
            {
                int nx = current.X + dx[i];
                int ny = current.Y + dy[i];

                if (nx < 0 || ny < 0 || nx >= Width || ny >= Height || visited[nx, ny])
                    continue;

                if (Grid[nx, ny] == CellType.Target)
                {
                    visited[nx, ny] = true;
                    queue.Enqueue(new CellPos(nx, ny));
                    connectedTargets++;
                }
            }
        }
        return connectedTargets == TargetCount;
    }
}

