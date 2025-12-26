namespace Core.Interfaces;

public interface IAudioCurve
{
    /// <summary>
    /// Maps a value from the input range to the output range
    /// </summary>
    /// <param name="value">Value ranging from 0 to 100</param>
    /// <returns>Value in the range of 0 to 1</returns>
    float Map(float value);
}