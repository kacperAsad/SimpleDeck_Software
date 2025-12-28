namespace Core.Config;

public class ActionConfig
{
    public string Type { get; set; } = "";
    public Dictionary<string, string> Parameters { get; set; } = new();
}