using Core.Input;
using Core.Interfaces;

namespace Core.Actions;

public class MediaNextAction : IAction
{
    public void Execute()
    {
        MediaKeysHelper.Next();
    }
}