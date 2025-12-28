using Core.Interfaces;

namespace Core.Actions;

public class MuteGlobalMicrophoneAction(IAudioService audioService) : IAction
{
    public void Execute()
    {
        audioService.ToggleGlobalMicrophoneMute();
    }
}