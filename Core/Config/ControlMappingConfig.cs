using Core.Interfaces;

namespace Core.Input;

public record ControlMappingConfig
{
    public string Control { get; init; } = "";// Vol1, Vol2, ...
    public string Target { get; init; } = "";// Master, Apps, Games
    public string? Process { get; init; }

    public string CurveType { get; init; } = "log"; // log / linear



}