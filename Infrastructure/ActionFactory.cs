using Core;
using Core.Actions;
using Core.Config;
using Core.Interfaces;

namespace Infrastructure;

public static class ActionFactory
{
    public static IAction Create(ActionConfig actionConfig, IMediaService mediaService, IAudioService audioService, IKeyboardSimulator keyboardSimulator)
    {
        return actionConfig.Type switch
        {
            ActionType.PlayPause => new MediaPlayPauseAction(mediaService),
            ActionType.PreviousTrack => new MediaPreviousAction(mediaService),
            ActionType.NextTrack => new MediaNextAction(mediaService),
            ActionType.Stop => new MediaStopAction(mediaService),
            ActionType.OpenApp => new SystemOpenAppAction(actionConfig),
            ActionType.MuteApp => new MediaMuteAppAction(actionConfig, audioService),
            ActionType.MuteMicrophone => new MuteGlobalMicrophoneAction(audioService),
            ActionType.KeyboardShortcut => new KeyboardShortcutAction(keyboardSimulator, actionConfig),
            
            _ => throw new NotSupportedException(actionConfig.Type.ToString())
        };
    }
}