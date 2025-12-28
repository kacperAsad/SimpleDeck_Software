using Core.Config;
using Core.Interfaces;

namespace Core.Routing;

public class ButtonMappingRuntime
{
    public string ControlId { get; set; } = "";
    
    public IAction Action { get; set; }
    
    public Dictionary<string, string> Params { get; set; } = new();
}