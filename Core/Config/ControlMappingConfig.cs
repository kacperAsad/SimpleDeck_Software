using System.Text.Json.Serialization;
using Core.Interfaces;

namespace Core.Input;

public record ControlMappingConfig
{
    public string Control { get; init; } = "";// Vol1, Vol2, ...
    
    [JsonConverter(typeof(JsonStringEnumConverter<PotentiometerTarget>))]
    public PotentiometerTarget Target { get; init; } // Master, Apps, Group
    public string? Process { get; init; }

    public string? GroupName { get; init; }

    public string CurveType { get; init; } = "log"; // log / linear



}