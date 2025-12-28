
using Core.Input;
using Core.Interfaces;

namespace Core.Routing;

public class InputRouter
{
    private readonly IAudioService _audioService;
    private Dictionary<string, ControlMappingRuntime> _mappings;
    
    private Dictionary<string, ButtonMappingRuntime> _buttonActions;

    public InputRouter(
        IAudioService audioService,
        IEnumerable<ControlMappingRuntime> mappings,
        IEnumerable<ButtonMappingRuntime> buttonActions)
    {
        _audioService = audioService;
        _mappings = mappings.ToDictionary(m => m.Control);
        _buttonActions = buttonActions.ToDictionary(m => m.ControlId);
    }

    public void Handle(DeviceMessage message)
    {
        

        if (message.Type.StartsWith("VOL"))  // Here is parsed volume channels
        {
            if (!_mappings.TryGetValue(message.Type, out var mapping)) return;
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

        } else if (message.Type.StartsWith("BTN"))
        {
            if (!_buttonActions.TryGetValue(message.Type, out var buttonAction)) return;
            if (message.Value != 1) return;
            buttonAction.Action.Execute();
        }
        
    }

    public void Reload(IEnumerable<ControlMappingRuntime> mappings)
    {
        _mappings = mappings.ToDictionary(m => m.Control);
    }
    
}