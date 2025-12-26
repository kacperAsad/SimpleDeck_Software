using Core.Interfaces;

namespace Core.Input;

public record ControlMapping
{
    public string Control { get; set; } = "";// Vol1, Vol2, ...
    public string Target { get; set; } = "";// Master, Apps, Games
    public string? Process { get; set; }

    public string CurveType { get; set; } = "linear"; // log / linear

    // Ten obiekt jest tworzony w Program.cs przez factory
    public IAudioCurve? AudioCurve { get; set; }
}