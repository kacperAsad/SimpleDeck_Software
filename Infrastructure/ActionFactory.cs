using Core;
using Core.Actions;
using Core.Config;
using Core.Interfaces;

namespace Infrastructure;

public static class ActionFactory
{
    public static IAction Create(ActionConfig actionConfig, IMediaService mediaService)
    {
        return actionConfig.Type switch
        {
            ActionType.PlayPause => new MediaPlayPauseAction(mediaService),
            ActionType.PreviousTrack => new MediaPreviousAction(mediaService),
            ActionType.NextTrack => new MediaNextAction(mediaService),
            ActionType.OpenApp => new SystemOpenAppAction(actionConfig),
            _ => throw new NotSupportedException(actionConfig.Type.ToString())
        };
    }
}