using Core.Audio;
using Core.Interfaces;

namespace Infrastructure;

public static class VolumeCurveFactory
{
    public static IAudioCurve Create(string type)
    {
        return type switch
        {
            "log" => new LogVolumeCurve(),
            "linear" => new LinearVolumeCurve(),
            _ => new LinearVolumeCurve()
        };
    }
}