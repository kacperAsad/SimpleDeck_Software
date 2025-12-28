namespace Core.Interfaces;

public interface IAudioService
{
    void SetApplicationVolume(string processName, float volume);
    void SetMasterVolume(float volume);
    
    float GetMasterVolume();

    void ToggleApplicationMute(string processName, bool? mute = null);
}