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
            "Play_Pause_Action" => new MediaPlayPauseAction(mediaService),
            "Previous_Action" => new MediaPreviousAction(mediaService),
            "Next_Action" => new MediaNextAction(mediaService),
            "Open_App_Action" => new SystemOpenAppAction(actionConfig),
            _ => throw new NotSupportedException(actionConfig.Type)
        };
    }
}