using Core.Interfaces;

namespace Audio;

public class SmoothAudioService : IAudioService, IDisposable
{
    private readonly IAudioService _inner;
    private readonly Dictionary<string, VolumeTarget> _targets = new();
    private readonly object _lock = new();
    private readonly Timer _updateTimer;

    private const float SmoothingSpeed = 0.3f; // Im większe, tym szybsze przejście (0.01 - 0.2)
    private const int UpdateIntervalMs = 20; // Częstotliwość aktualizacji

    public SmoothAudioService(IAudioService inner)
    {
        _inner = inner;
        _updateTimer = new Timer(Update, null, 0, UpdateIntervalMs);
    }

    public void SetApplicationVolume(string processName, float targetVolume)
    {
        lock (_lock)
        {
            var key = $"app:{processName}";

            if (!_targets.ContainsKey(key))
            {
                _targets[key] = new VolumeTarget
                {
                    Type = VolumeTargetType.Application,
                    ProcessName = processName,
                    CurrentVolume = targetVolume,
                    TargetVolume = targetVolume
                };
            }
            else
            {
                _targets[key].TargetVolume = targetVolume;
            }
        }
    }

    public void SetMasterVolume(float targetVolume)
    {
        lock (_lock)
        {
            const string key = "master";

            if (!_targets.ContainsKey(key))
            {
                _targets[key] = new VolumeTarget
                {
                    Type = VolumeTargetType.Master,
                    CurrentVolume = _inner.GetMasterVolume(),
                    TargetVolume = targetVolume
                };
            }
            else
            {
                _targets[key].TargetVolume = targetVolume;
            }
        }
    }

    public float GetMasterVolume()
    {
        return _inner.GetMasterVolume();
    }

    public void ToggleApplicationMute(string processName, bool? mute = null)
    {
        throw  new NotImplementedException();
    }

    public void ToggleGlobalMicrophoneMute(bool? mute = null)
    {
        throw  new NotImplementedException();
    }

    private void Update(object? state)
    {
        lock (_lock)
        {
            foreach (var target in _targets.Values)
            {
                // Oblicz różnicę
                float diff = target.TargetVolume - target.CurrentVolume;

                // Jeśli różnica jest bardzo mała, ustaw od razu na target
                if (Math.Abs(diff) < 0.001f)
                {
                    target.CurrentVolume = target.TargetVolume;
                }
                else
                {
                    // Smooth interpolation (exponential ease-out)
                    target.CurrentVolume += diff * SmoothingSpeed;
                }

                // Ustaw rzeczywistą głośność
                switch (target.Type)
                {
                    case VolumeTargetType.Application:
                        _inner.SetApplicationVolume(target.ProcessName!, target.CurrentVolume);
                        break;
                    case VolumeTargetType.Master:
                        _inner.SetMasterVolume(target.CurrentVolume);
                        break;
                }
            }
        }
    }

    public void Dispose()
    {
        _updateTimer?.Dispose();
    }

    private class VolumeTarget
    {
        public VolumeTargetType Type { get; set; }
        public string? ProcessName { get; set; }
        public float CurrentVolume { get; set; }
        public float TargetVolume { get; set; }
    }

    private enum VolumeTargetType
    {
        Master,
        Application
    }
}
