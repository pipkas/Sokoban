namespace Sokoban.Controller;

public static class UserFileManager
{
    private static string prefix = "player_";
    public static string GetUserFilePath(string nickname, string baseDir)
    {
        string fileName = $"{prefix}{nickname}.json";
        return Path.Combine(baseDir, fileName);
    }

    public static string GetUserNickName(string filePath)
    {
        string fileName = Path.GetFileName(filePath);
        if (!fileName.StartsWith(prefix)) 
            return string.Empty;

        return Path.GetFileNameWithoutExtension(fileName.Substring(prefix.Length));
    }

}