using System.Text.Json;
using Sokoban.Models;
using Sokoban.Logging;

namespace Sokoban.Controller;

public static class LoadManager
{
    public static List<GameLevel> LoadGameLevels(string dirPath)
    {
        var levels = new List<GameLevel>();
        try
        {
            var files = Directory.GetFiles(dirPath, "*.json");
            foreach (var filePath in files)
            {
                try
                {
                    var json = File.ReadAllText(filePath);
                    var gameLevel = JsonSerializer.Deserialize<GameLevel>(json);
                    
                    if (gameLevel != null)
                    {
                        bool nameExists = levels.Any(l => 
                            string.Equals(l.Name, gameLevel.Name, StringComparison.OrdinalIgnoreCase));
                        
                        if (nameExists)
                            Log.Warning($"Duplicate level name '{gameLevel.Name}' found in {filePath}.");
                        else
                            levels.Add(gameLevel); 
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error loading {filePath}: {ex.Message}");
                }
            }
        }
        catch (DirectoryNotFoundException)
        {
            Log.Error($"Directory not found: {dirPath}");
        }
        return levels;
    }

    public static List<LevelData> LoadLevelsData(string dirPath)
    {
        var levelsData = new List<LevelData>();
        var gameLevels = LoadGameLevels(dirPath);
        foreach(var gameLevel in gameLevels)
        {
            var levelData = new LevelData(gameLevel);
            if (levelData.IsValid)
                levelsData.Add(levelData);
        }
        return levelsData;
    }

    public static Dictionary<string, LevelStats>? LoadLevelStats(string nickname, string dirPath)
    {
        try
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            
            var filePath = UserFileManager.GetUserFilePath(nickname, dirPath);
            if (!File.Exists(filePath))
                return [];
            
            var json = File.ReadAllText(filePath);
            var levelStats = JsonSerializer.Deserialize<Dictionary<string, LevelStats>>(json, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
            return levelStats;
        }
        catch (Exception ex)
        {
            Log.Error($"Error loading LevelStats: {ex.Message}");
            return null;
        }
    }

    public static List<User> LoadUsersProgress(string dirPath)
    {
        var files = Directory.GetFiles(dirPath);
        var users = new List<User>();

        foreach (var filePath in files)
        {
            string nickname = UserFileManager.GetUserNickName(filePath);
            if (nickname == string.Empty)
            {
                Log.Warning($"in {dirPath} wrong name of user file");
                continue;
            }
            users.Add(new User(nickname, LoadLevelStats(filePath)));
        }
        return users;
    }

    private static Dictionary<string, LevelStats>? LoadLevelStats(string filePath)
    {
        try{
            var json = File.ReadAllText(filePath);
            var levelStats = JsonSerializer.Deserialize<Dictionary<string, LevelStats>>(json, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
            return levelStats;
            }
        catch (Exception ex)
        {
            Log.Error($"Error loading LevelStats: {ex.Message}");
            return null;
        }
    }
}