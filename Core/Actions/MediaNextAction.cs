using Core.Input;
using Core.Interfaces;


namespace Core.Actions;

public class MediaNextAction(IMediaService mediaService) : IAction
{
    public void Execute()
    {
        mediaService.Next();
    }
}