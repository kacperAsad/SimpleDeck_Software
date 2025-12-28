using Core.Input;
using Core.Interfaces;

namespace Core.Actions;

public class MediaPreviousAction : IAction
{
    public void Execute()
    {
        MediaKeysHelper.Previous();
    }
}