using Core.Interfaces;

namespace Core.Audio;

public class LogVolumeCurve : IAudioCurve
{
    private const float MinDb = -30f;  // Zmniejszony z -60 dla płynniejszej krzywej
    
    public float Map(float value)
    {
        if (value <= 0) return 0;

        float linear = value / 100f;

        float db = MinDb + (linear * (-MinDb));
        
        return (float)Math.Pow(10, db / 20f);
    }
}