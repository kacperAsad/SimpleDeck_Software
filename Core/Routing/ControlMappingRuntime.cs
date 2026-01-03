using Core.Interfaces;

namespace Core.Routing;

public class ControlMappingRuntime(string control, PotentiometerTarget target, string? process, IAudioCurve audioCurve, string? group = null)
{
    public string Control { get; } = control;
    public PotentiometerTarget Target { get; } = target;
    public string? Process { get; } = process;
    public IAudioCurve AudioCurve { get; } = audioCurve;
    public string? Group { get; set; } = group;
}