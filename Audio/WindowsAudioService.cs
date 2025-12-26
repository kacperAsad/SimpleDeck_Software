using System.Diagnostics;
using Core.Interfaces;
using NAudio.CoreAudioApi;

namespace Audio;

public class WindowsAudioService : IAudioService
{

    private readonly MMDevice _device;

    public WindowsAudioService()
    {
        var enumerator = new MMDeviceEnumerator();
        
        _device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
    }
    
    public void SetApplicationVolume(string processName, float volume)
    {
        var regulatedVolume = Math.Clamp(volume, 0f, 1f);
        
        var sessions = _device.AudioSessionManager.Sessions;

        for (int i = 0; i < sessions.Count; i++)
        {
            var session = sessions[i];
            var pid = session.GetProcessID;

            try
            {
                var process = Process.GetProcessById((int)pid);
                
                if (!process.ProcessName.Equals(processName, StringComparison.OrdinalIgnoreCase)) continue;
                
                session.SimpleAudioVolume.Volume = regulatedVolume;
            }
            catch
            {
                // ignored
            }
        }
    }

    public void SetMasterVolume(float volume)
    {
        var regulatedVolume = Math.Clamp(volume, 0f, 1f);
        _device.AudioEndpointVolume.MasterVolumeLevelScalar = regulatedVolume;
    }

    public float GetMasterVolume()
    {
        return _device.AudioEndpointVolume.MasterVolumeLevelScalar;
    }
}