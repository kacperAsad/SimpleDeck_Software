using Core.Interfaces;

namespace Core.Actions;

public class MediaMuteAppAction(string appName, IAudioService service) :IAction
{
    public void Execute()
    {
        service.ToggleApplicationMute(appName);
    }
}