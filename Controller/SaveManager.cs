using System.Text.Json;
using Sokoban.Models;
using Sokoban.Logging;

namespace Sokoban.Controller;

public static class SaveManager
{
    public static bool SaveLevelStats(User user, string dirPath)
    {
        try
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            
            var filePath = UserFileManager.GetUserFilePath(user.Nickname, dirPath);
            
            var options = new JsonSerializerOptions
            { 
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            var json = JsonSerializer.Serialize(user.LevelsStats, options);
            File.WriteAllText(filePath, json);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error($"Error saving LevelStats for player '{user.Nickname}': {ex.Message}");
            return false;
        }
    }
}