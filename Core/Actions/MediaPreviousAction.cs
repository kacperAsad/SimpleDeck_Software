using Core.Input;
using Core.Interfaces;


namespace Core.Actions;

public class MediaPreviousAction (IMediaService mediaService) : IAction
{
    public void Execute()
    {
        mediaService.Previous();
    }
}