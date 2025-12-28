using Core.Input;
using Core.Interfaces;


namespace Core.Actions;

public class MediaPlayPauseAction (IMediaService _mediaService) : IAction
{
    public void Execute()
    {
        _mediaService.PlayPause();
    }
}