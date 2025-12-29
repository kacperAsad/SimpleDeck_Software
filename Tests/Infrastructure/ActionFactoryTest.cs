using Core;
using Core.Actions;
using Core.Config;
using Core.Interfaces;
using Infrastructure;
using Moq;

namespace Tests.Infrastructure;

public class ActionFactoryTest
{
    private readonly Mock<IMediaService> _mediaService = new();
    private readonly Mock<IAudioService> _audioService = new();
    private readonly Mock<IKeyboardSimulator> _keyboardService = new();


    [Fact]
    public void Create_PlayPauseType_ReturnsPlayPauseType()
    {
        // Arrange
        var config = new ActionConfig { Type = ActionType.PlayPause };
        
        // Act
        var result = ActionFactory.Create(config, _mediaService.Object, _audioService.Object, _keyboardService.Object);
        
        // Assert
        Assert.IsType<MediaPlayPauseAction>(result);
    }

    [Fact]
    public void Create_StopType_ReturnsStopType()
    {
        // Arrange
        var config = new ActionConfig { Type = ActionType.Stop };
        
        // Act
        var result = ActionFactory.Create(config, _mediaService.Object, _audioService.Object, _keyboardService.Object);
        
        // Assert 
        Assert.IsType<MediaStopAction>(result);
    }

    [Fact]
    public void Create_NextTrackType_ReturnsNextTrackType()
    {
        // Arrange
        var config = new ActionConfig { Type = ActionType.NextTrack };
        
        // Act
        var result = ActionFactory.Create(config, _mediaService.Object, _audioService.Object, _keyboardService.Object);
        
        // Assert
        Assert.IsType<MediaNextAction>(result);
    }
}