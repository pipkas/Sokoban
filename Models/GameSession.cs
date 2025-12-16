using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using Sokoban.Models;
using Sokoban.Logging;

namespace Sokoban.Models;

public class GameSession
{
    public LevelStatus Level;
    public int MoveCount = 0;
    public Player Player;
    private readonly Stopwatch stopwatch = new();
    public bool IsPaused {get; private set;} = false;
    public bool IsGameOver {get; private set;} = false;
    public bool IsWin {get; private set;} = false;
    private TimeSpan finalTime = TimeSpan.Zero;

    public TimeSpan ElapsedTime 
    {
        get
        {
            if (IsGameOver)
                return finalTime;
            return stopwatch.Elapsed;
        }
    }

    public GameSession(LevelData levelData)
    {
        Player = new Player(levelData.PlayerPos);
        Level = new LevelStatus(levelData);
        if (!levelData.IsValid){
            Log.Error($"Level {levelData.Name} is not valid");
            return;
        }
    }
    public void Start()
    {
        if (!stopwatch.IsRunning)
        {
            stopwatch.Start();
        }
        IsPaused = false;
    }
    public void Pause()
    {
        IsPaused = true;
        stopwatch.Stop();
    }
    public void Resume()
    {
        IsPaused = false;
        stopwatch.Start();
    }
    public bool TryMove(Move direction)
    {
        if (IsPaused) return false; 
        var nextCell = Player.Position.Move(direction);
        var isBox = Level.IsBox(nextCell);
        var isFree = Level.IsFreeCell(nextCell);
        if (isFree || isBox && Level.MoveBox(nextCell, direction))
        {
            if (isBox && Level.IsCurBoxInTarget && CheckGameOver())
            {
                finalTime = stopwatch.Elapsed;
                EndGame(true);
            }
            Player.MoveTo(nextCell);
            MoveCount++;
            return true;
        }
        return false;
    }
    public void EndGame(bool isWin)
    {
        stopwatch.Stop();
        IsGameOver = true;
        IsWin = isWin;
    }
    private bool CheckGameOver() => Level.IsCompleted();
}