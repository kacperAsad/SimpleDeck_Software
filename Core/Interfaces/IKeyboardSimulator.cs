namespace Core.Interfaces;

public interface IKeyboardSimulator
{
    void SendShortcut(IEnumerable<int> virtualKeys);
}