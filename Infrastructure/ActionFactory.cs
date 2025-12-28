using Core.Actions;
using Core.Config;
using Core.Interfaces;

namespace Infrastructure;

public static class ActionFactory
{
    public static IAction Create(ActionConfig actionConfig)
    {
        return actionConfig.Type switch
        {
            "Play_Pause_Action" => new MediaPlayPauseAction(),
            "Previous_Action" => new MediaPreviousAction(),
            "Next_Action" => new MediaNextAction(),
            "Open_App_Action" => new SystemOpenAppAction(actionConfig),
            _ => throw new NotSupportedException(actionConfig.Type)
        };
    }
}