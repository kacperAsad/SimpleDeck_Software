using Core.Input;
using Core.Routing;

namespace Infrastructure;

public class ControlMappingFactory
{
    public static ControlMappingRuntime Create(ControlMappingConfig config)
    {
        var curve = VolumeCurveFactory.Create(config.CurveType);

        return new ControlMappingRuntime(
            config.Control,
            config.Target,
            config.Process,
            curve,
            config.GroupName
        );
    }
}