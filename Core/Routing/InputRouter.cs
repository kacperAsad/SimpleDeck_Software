
using Core.Input;
using Core.Interfaces;

namespace Core.Routing;

public class InputRouter
{
    private readonly IAudioService _audioService;
    private Dictionary<string, ControlMapping> _mappings;

    public InputRouter(
        IAudioService audioService,
        IEnumerable<ControlMapping> mappings)
    {
        _audioService = audioService;
        _mappings = mappings.ToDictionary(m => m.Control);
    }

    public void Handle(DeviceMessage message)
    {
        if (!_mappings.TryGetValue(message.Type, out var mapping)) return;

        if (message.Type.StartsWith("VOL"))  // Here is parsed volume channels
        {
            // Użyj curve jeśli jest dostępna, w przeciwnym razie liniowa konwersja
            float volume = mapping.AudioCurve?.Map(message.Value) ?? (message.Value / 100f);

            switch (mapping.Target)
            {
                case "Application":
                    _audioService.SetApplicationVolume(mapping.Process!, volume);
                    break;
                case "Master":
                    _audioService.SetMasterVolume(volume);
                    break;
                case "Game":
                    // Todo dodać osobno gry, aby dało się automatycznie przełączać i wykrywać obecną grę
                    break;
            }

        }
    }

    public void Reload(IEnumerable<ControlMapping> mappings)
    {
        _mappings = mappings.ToDictionary(m => m.Control);
    }
    
}