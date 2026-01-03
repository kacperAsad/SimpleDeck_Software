using System.Collections.Concurrent;
using System.Diagnostics;
using Core.Interfaces;
using NAudio.CoreAudioApi;

namespace Audio;

public class WindowsAudioService : IAudioService, IDisposable
{
    private MMDevice _device;
    private readonly MMDevice _microphoneDevice;
    private readonly MMDeviceEnumerator _enumerator;
    
    private readonly ConcurrentDictionary<string, List<AudioSessionControl>> _sessionCache = new(StringComparer.OrdinalIgnoreCase);
    private readonly Timer _cacheTimer;

    public WindowsAudioService()
    {
        _enumerator = new MMDeviceEnumerator();
        
        // Inicjalizacja urządzeń
        _device = _enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        _microphoneDevice = _enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);

        // Odświeżanie co 1s
        _cacheTimer = new Timer(_ => RefreshCache(), null, 0, 1000);
    }

    private void RefreshCache()
    {
        try
        {
            // 1. Sprawdź, czy domyślne urządzenie się nie zmieniło (np. podpięcie słuchawek)
            var currentDevice = _enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            if (currentDevice.ID != _device.ID)
            {
                Console.WriteLine($"[Audio] Zmiana urządzenia wyjściowego na: {currentDevice.FriendlyName}");
                _device = currentDevice;
            }

            // 2. Pobierz aktualne sesje
            var sessionManager = _device.AudioSessionManager;
            sessionManager.RefreshSessions(); // Wymuś odświeżenie listy sesji
            var sessions = sessionManager.Sessions;

            var newCache = new Dictionary<string, List<AudioSessionControl>>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < sessions.Count; i++)
            {
                var session = sessions[i];
                var pid = session.GetProcessID;
                
                if (pid == 0) continue; // Pomiń systemowe dźwięki bez PID

                try
                {
                    string name;
                    using (var process = Process.GetProcessById((int)pid))
                    {
                        name = process.ProcessName;
                    }

                    if (!newCache.ContainsKey(name))
                        newCache[name] = new List<AudioSessionControl>();
                    
                    newCache[name].Add(session);
                }
                catch
                {
                    // Sesja może należeć do procesu, który właśnie się zamknął
                }
            }

            // 3. Atomowa podmiana cache'u (zamiast Clear(), co zapobiega błędom podczas odczytu)
            // Najpierw usuwamy stare sesje (opcjonalnie, NAudio dba o nie, ale to dobra praktyka)
            _sessionCache.Clear();
            foreach (var kvp in newCache)
            {
                _sessionCache[kvp.Key] = kvp.Value;
            }
            
            // DEBUG: Odkomentuj to, żeby zobaczyć w konsoli co widzi program
            // Console.WriteLine($"[Audio Cache] Wykryte aplikacje: {string.Join(", ", _sessionCache.Keys)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Audio Error] Błąd podczas odświeżania cache: {ex.Message}");
        }
    }

    public void SetApplicationVolume(string processName, float volume)
    {
        if (string.IsNullOrEmpty(processName)) return;
        var regulatedVolume = Math.Clamp(volume, 0f, 1f);

        if (_sessionCache.TryGetValue(processName, out var sessions))
        {
            foreach (var session in sessions)
            {
                try
                {
                    if (Math.Abs(session.SimpleAudioVolume.Volume - regulatedVolume) > 0.001f)
                    {
                        session.SimpleAudioVolume.Volume = regulatedVolume;
                    }
                }
                catch { /* Sesja mogła wygasnąć */ }
            }
        }
    }

    public void SetMasterVolume(float volume)
    {
        try
        {
            _device.AudioEndpointVolume.MasterVolumeLevelScalar = Math.Clamp(volume, 0f, 1f);
        }
        catch { }
    }

    public float GetMasterVolume() => _device.AudioEndpointVolume.MasterVolumeLevelScalar;

    public void ToggleApplicationMute(string processName, bool? mute = null)
    {
        if (_sessionCache.TryGetValue(processName, out var sessions))
        {
            foreach (var session in sessions)
            {
                try { session.SimpleAudioVolume.Mute = mute ?? !session.SimpleAudioVolume.Mute; } catch { }
            }
        }
    }

    public void ToggleGlobalMicrophoneMute(bool? mute = null)
    {
        try { _microphoneDevice.AudioEndpointVolume.Mute = mute ?? !_microphoneDevice.AudioEndpointVolume.Mute; } catch { }
    }

    public IEnumerable<string> GetActiveAudioProcesses() => _sessionCache.Keys;

    public void Dispose()
    {
        _cacheTimer?.Dispose();
        _device?.Dispose();
        _microphoneDevice?.Dispose();
        _enumerator?.Dispose();
    }
}