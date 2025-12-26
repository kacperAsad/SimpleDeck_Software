using Core.Interfaces;

namespace Core.Audio;

public class LinearVolumeCurve : IAudioCurve
{
    public float Map(float value)
    {
        return Math.Clamp(value / 100, 0, 1);
    }
}