using Core.Config;
using Core.Routing;

namespace Infrastructure;

public static class ButtonMappingRuntimeFactory
{
    public static ButtonMappingRuntime Create(ButtonMappingConfig buttonMappingConfig)
    {
        return new ButtonMappingRuntime()
        {
            ControlId = buttonMappingConfig.ControlId,
            Params = buttonMappingConfig.Action.Parameters,
            Action = ActionFactory.Create(buttonMappingConfig.Action)
        };
    }
}