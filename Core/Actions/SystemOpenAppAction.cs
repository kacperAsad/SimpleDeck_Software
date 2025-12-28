using System.Diagnostics;
using Core.Audio;
using Core.Config;
using Core.Interfaces;

namespace Core.Actions;

public class SystemOpenAppAction(ActionConfig actionConfig) : IAction
{
    private readonly ActionConfig _actionConfig = actionConfig;

    public void Execute()
    {
        _actionConfig.Parameters.TryGetValue("path", out var path);
        if  (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path)) return;

        var startInfo = new ProcessStartInfo()
        {
            FileName = path,
            UseShellExecute = true
        };

        try
        {
            if (Path.IsPathRooted(path) && File.Exists(path))
            {
                startInfo.WorkingDirectory = Path.GetDirectoryName(path);
            }
        }
        catch
        {
            // If steam uri presented
            //   steam://rungameid/412220
            // Then ignore setting working dir
        }
        
        
        try
        {
            Process.Start(startInfo);
        } catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}