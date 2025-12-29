using Core;
using Core.Config;
using Core.Input;
using Core.Interfaces;
using Core.Routing;
using Infrastructure;

namespace Tests.Core.Routing;

using Moq;
using Xunit;

public class InputRouterTests
{
    private readonly Mock<IAudioService> _audioServiceMock = new();

    [Fact]
    public void Handle_ButtonMessage_ValueIsOne_ExecutesAction()
    {
        // Arrange
        var actionMock = new Mock<IAction>();
        var buttonRuntime = new ButtonMappingRuntime
        {
            ControlId = "BTN1",
            Action = actionMock.Object
        };

        // Tworzymy router z jedną akcją przycisku
        var sut = new InputRouter(
            _audioServiceMock.Object, 
            [],
            [buttonRuntime]);

        var message = new DeviceMessage ( "BTN1", 1 );
        

        // Act
        sut.Handle(message);

        // Assert
        actionMock.Verify(a => a.Execute(), Times.Once);
    }

    [Fact]
    public void Handle_ButtonMessage_ValueIsZero_DoesNotExecuteAction()
    {
        // Arrange
        var actionMock = new Mock<IAction>();
        var buttonRuntime = new ButtonMappingRuntime { ControlId = "BTN1", Action = actionMock.Object };
        var sut = new InputRouter(_audioServiceMock.Object, Enumerable.Empty<ControlMappingRuntime>(), new[] { buttonRuntime });
        
        // Symulujemy puszczenie przycisku (Value = 0)
        var message = new DeviceMessage ( "BTN1",  0 );

        // Act
        sut.Handle(message);

        // Assert
        actionMock.Verify(a => a.Execute(), Times.Never);
    }

    [Fact]
    public void Handle_VolumeMasterMessage_CallsSetMasterVolume()
    {
        // Arrange
        var controlRuntime = new ControlMappingRuntime
        (
            "VOL1",
            "Master",
            null,
            null
            // Nie ustawiamy krzywej, więc pójdzie fallback: value / 100f
        );

        var sut = new InputRouter(
            _audioServiceMock.Object, 
            new[] { controlRuntime }, 
            Enumerable.Empty<ButtonMappingRuntime>());

        var message = new DeviceMessage( "VOL1", 50 ); // 50 / 100 = 0.5f

        // Act
        sut.Handle(message);

        // Assert
        _audioServiceMock.Verify(a => a.SetMasterVolume(0.5f), Times.Once);
    }

    [Fact]
    public void Handle_VolumeApplicationMessage_CallsSetApplicationVolume()
    {
        // Arrange
        var controlRuntime = new ControlMappingRuntime
        (
            "VOL2",
            "Application",
            "spotify",
            null
        );

        var sut = new InputRouter(
            _audioServiceMock.Object, 
            new[] { controlRuntime }, 
            Enumerable.Empty<ButtonMappingRuntime>());

        var message = new DeviceMessage ("VOL2", 80 );

        // Act
        sut.Handle(message);

        // Assert
        _audioServiceMock.Verify(a => a.SetApplicationVolume("spotify", 0.8f), Times.Once);
    }

    [Fact]
    public void Handle_VolumeWithCurve_UsesCurveToCalculateVolume()
    {
        // Arrange
        var curveMock = new Mock<IAudioCurve>();
        // Symulujemy, że krzywa dla wartości 100 zwraca 0.25f (bardzo cicho)
        curveMock.Setup(c => c.Map(100)).Returns(0.25f);

        var controlRuntime = new ControlMappingRuntime
        (
            "VOL3",
            "Master",
            null,
            curveMock.Object
            
        );

        var sut = new InputRouter(_audioServiceMock.Object, new[] { controlRuntime }, Enumerable.Empty<ButtonMappingRuntime>());
        var message = new DeviceMessage ("VOL3",  100 );

        // Act
        sut.Handle(message);

        // Assert
        _audioServiceMock.Verify(a => a.SetMasterVolume(0.25f), Times.Once);
    }
}