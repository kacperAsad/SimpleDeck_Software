namespace Core.Config;

public class AppGroupConfig
{
    public string Name { get; set; } = "";                  // Group name ex. "Games", "Browsers" 
    public List<string> Processes { get; set; } = new();    // App Names: "minecraft", "LeagueOfLegends"
}