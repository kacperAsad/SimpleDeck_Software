using Core.Config;
using Core.Interfaces;

namespace Core.Actions;

public class KeyboardShortcutAction(IKeyboardSimulator _keyboardSimulator, ActionConfig config) : IAction
{
    
    private List<int> _keys = new();
    public void Execute()
    {
        if (_keys.Count == 0) GetKeys();

        if (_keys.Any())
        {
            _keyboardSimulator.SendShortcut(_keys);
        }
    }
    
    private void GetKeys()
    {
        try
        {
            if (config.Parameters.TryGetValue("keys", out var keys))
            {
                _keys = keys.Split(',')
                    .Select(s => s.Trim())
                    .Select(s => s.StartsWith("0x")
                        ? Convert.ToInt32(s, 16)
                        : int.Parse(s))
                    .ToList();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // TODO Dodać Logger
        }
    }
    
}