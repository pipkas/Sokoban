

namespace Sokoban.Models;

public record LevelAttempt
{
    public TimeSpan Time { get; init; }
    public int Steps { get; init; }
    public DateTime CompletionDate { get; init; }
}