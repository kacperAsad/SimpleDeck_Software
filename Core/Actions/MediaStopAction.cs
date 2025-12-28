using Core.Interfaces;

namespace Core.Actions;

public class MediaStopAction(IMediaService mediaService) : IAction
{
    public void Execute()
    {
        mediaService.Stop();
    }
}