
using Core.Config;
using Core.Input;
using Core.Interfaces;
using Core.Services;

namespace Core.Routing;

public class InputRouter
{
    private readonly IAudioService _audioService;
    private Dictionary<string, ControlMappingRuntime> _mappings;
    
    private Dictionary<string, ButtonMappingRuntime> _buttonActions;
    
    private readonly ProcessResolver _processResolver;
    private readonly List<AppGroupConfig> _groups;

    public InputRouter(
        IAudioService audioService,
        ProcessResolver processResolver,
        IEnumerable<ControlMappingRuntime> mappings,
        IEnumerable<ButtonMappingRuntime> buttonActions,
        IEnumerable<AppGroupConfig>? groups)
    {
        _audioService = audioService;
        _mappings = mappings.ToDictionary(m => m.Control);
        _buttonActions = buttonActions.ToDictionary(m => m.ControlId);
        _processResolver = processResolver;
        _groups = (List<AppGroupConfig>?)groups ?? [];
    }

    public void Handle(DeviceMessage message)
    {
        

        if (message.Type.StartsWith("VOL"))  // Here is parsed volume channels
        {
            if (!_mappings.TryGetValue(message.Type, out var mapping)) return;
            // Użyj curve, jeśli jest dostępna, w przeciwnym razie liniowa konwersja
            float volume = mapping.AudioCurve?.Map(message.Value) ?? (message.Value / 100f);

            switch (mapping.Target)
            {
                case PotentiometerTarget.Application:
                    _audioService.SetApplicationVolume(mapping.Process!, volume);
                    break;
                case PotentiometerTarget.Master:
                    _audioService.SetMasterVolume(volume);
                    break;
                case PotentiometerTarget.Group:
                    var group = _groups.FirstOrDefault(g => g.Name.Equals(mapping.Group!, StringComparison.OrdinalIgnoreCase));
                    
                    if (group != null)
                    {
                        var activeProcess = _processResolver.ResolveActiveProcessFromGroup(mapping.Group!, group.Processes);
                        
                        if (activeProcess != null && !string.IsNullOrEmpty(activeProcess.Trim()))
                        {

                            _audioService.SetApplicationVolume(activeProcess, volume);
                        }
                    }
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