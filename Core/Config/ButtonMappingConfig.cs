namespace Core.Config;

public class ButtonMappingConfig
{
    public string ControlId { get; set; } = "";
    public ActionConfig Action { get; set; } = new();
}