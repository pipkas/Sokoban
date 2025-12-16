namespace Sokoban.Models;

public record LevelStats
{
    public TimeSpan BestTime { get; set; }
    public List<LevelAttempt> Attempts { get; set; } = new();
}