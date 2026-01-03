namespace Core.Interfaces;

public interface IAudioService : IDisposable
{
    void SetApplicationVolume(string processName, float volume);
    void SetMasterVolume(float volume);
    
    float GetMasterVolume();

    void ToggleApplicationMute(string processName, bool? mute = null);

    public void ToggleGlobalMicrophoneMute(bool? mute = null);

    public IEnumerable<string> GetActiveAudioProcesses();
}