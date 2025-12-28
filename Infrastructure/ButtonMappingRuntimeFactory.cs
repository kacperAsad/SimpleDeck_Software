using Core.Actions;
using Core.Config;
using Core.Interfaces;
using Core.Routing;

namespace Infrastructure;

public static class ButtonMappingRuntimeFactory
{
    public static ButtonMappingRuntime Create(ButtonMappingConfig buttonMappingConfig, IMediaService mediaService, IAudioService audioService)
    {
        return new ButtonMappingRuntime()
        {
            ControlId = buttonMappingConfig.ControlId,
            Params = buttonMappingConfig.Action.Parameters,
            Action = ActionFactory.Create(buttonMappingConfig.Action, mediaService, audioService)
        };
    }
}