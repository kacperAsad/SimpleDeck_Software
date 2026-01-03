using Core.Input;

namespace Core.Config;

public class AppConfig
{
    public List<ControlMappingConfig> Mappings { get; set; } = new();
    public List<ButtonMappingConfig> Buttons { get; set; } = new();

    public List<AppGroupConfig> AppGroups { get; set; } = new();
}