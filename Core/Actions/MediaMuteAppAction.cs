using Core.Config;
using Core.Interfaces;

namespace Core.Actions;

public class MediaMuteAppAction(ActionConfig config, IAudioService service) :IAction
{
    public void Execute()
    {
        // TODO Coś lepszego niż zwykły string
        config.Parameters.TryGetValue("ProcessName", out var processName);
        if (processName is not null)
            service.ToggleApplicationMute(processName);
        
    }
}