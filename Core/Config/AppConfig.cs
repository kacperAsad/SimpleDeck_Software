using Core.Input;

namespace Core.Config;

public class AppConfig
{
    public List<ControlMappingConfig> Mappings { get; set; } = new();
}