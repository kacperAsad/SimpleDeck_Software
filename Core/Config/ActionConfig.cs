using System.Text.Json.Serialization;

namespace Core.Config;

public class ActionConfig
{
    [JsonConverter(typeof(JsonStringEnumConverter<ActionType>))]
    public ActionType Type { get; set; } = ActionType.Unknown;
    public Dictionary<string, string> Parameters { get; set; } = new();
}