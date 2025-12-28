using Core.Interfaces;

namespace Core.Routing;

public class ControlMappingRuntime
{
    public string Control { get; }
    public string Target { get; }
    public string? Process { get; }
    public IAudioCurve AudioCurve { get; }

    public ControlMappingRuntime(string control, string target, string? process, IAudioCurve audioCurve)
    {
        Control = control;
        Target = target;
        Process = process;
        AudioCurve = audioCurve;
    }
    
}