using System.Diagnostics;
using Core.Interfaces;
using System.Linq;

namespace Core.Services;

public class ProcessResolver(IAudioService audioService)
{
    
    private readonly Dictionary<string, (string ProcessName, DateTime Expiry)> _resolvedCache = new();
    private readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(2);
    
    public string? ResolveActiveProcessFromGroup(string groupName, List<string> groupProcesses)
    {
        if (_resolvedCache.TryGetValue(groupName, out var cached) && DateTime.Now < cached.Expiry)
        {
            return cached.ProcessName;
        }

        var activeApps = audioService.GetActiveAudioProcesses();

        // ZMIANA: Używamy metody z LINQ, która obsługuje Comparer
        var winner = groupProcesses.FirstOrDefault(p => 
            activeApps.Contains(p, StringComparer.OrdinalIgnoreCase));

        if (winner == null)
        {
            winner = groupProcesses.FirstOrDefault(p => Process.GetProcessesByName(p).Length > 0);
        }

        if (winner != null)
        {
            _resolvedCache[groupName] = (winner, DateTime.Now.Add(CacheDuration));
        }

        return winner;
    }
    
}