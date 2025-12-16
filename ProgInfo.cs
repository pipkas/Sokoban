using Sokoban.Models;

namespace Sokoban;

public class ProgInfo(Settings settings, User user)
{
    public Settings Settings {get; } = settings;
    public User User {get; set;} = user;
}