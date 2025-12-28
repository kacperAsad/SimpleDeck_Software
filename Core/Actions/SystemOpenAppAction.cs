using System.Diagnostics;
using Core.Config;
using Core.Interfaces;

namespace Core.Actions;

public class SystemOpenAppAction(ActionConfig actionConfig) : IAction
{
    private readonly ActionConfig _actionConfig = actionConfig;

    public void Execute()
    {
        _actionConfig.Parameters.TryGetValue("path", out var path);
        if  (string.IsNullOrEmpty(path)) return;
        
        
        try
        {
            Process.Start(path);
        } catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}