using Core.Input;
using Core.Interfaces;

namespace Core.Actions;

public class MediaPlayPauseAction : IAction
{
    public void Execute()
    {
        MediaKeysHelper.PlayPause();
    }
}