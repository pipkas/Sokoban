namespace Sokoban.Models;

public class User
{
    //public string Name { get; set; }
    public string Nickname { get; set; }
    public string PlayerImage {get; set;} = "playerMan.png";
    public Dictionary<string, LevelStats> LevelsStats { get; set; }

    public User(string nickname, Dictionary<string, LevelStats>? levelsStats)
    {
        //Name = name;
        Nickname = nickname;
        LevelsStats = levelsStats ?? new Dictionary<string, LevelStats>();
    }

    public LevelStats? GetLevelStats(string levelName)
    {
        LevelsStats.TryGetValue(levelName, out var stats);
        return stats;
    }

    public void AddSuccessfulAttempt(string levelName, TimeSpan time, int steps)
    {
        if (GetLevelStats(levelName) == null)
            LevelsStats[levelName] = new LevelStats();
        
        var stats = LevelsStats[levelName];
        var attempt = new LevelAttempt
        {
            Time = time,
            Steps = steps,
            CompletionDate = DateTime.Now
        };
        
        stats.Attempts.Add(attempt);

        stats.BestTime = time < stats.BestTime || stats.BestTime == TimeSpan.Zero ? time : stats.BestTime;
    }
}