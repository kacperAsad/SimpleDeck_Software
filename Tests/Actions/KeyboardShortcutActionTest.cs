using Core.Actions;
using Core.Config;
using Core.Interfaces;
using Moq;

namespace Tests.Actions;

public class KeyboardShortcutActionTest
{
    private readonly Mock<IKeyboardSimulator> _keyboardSimulatorMock;
    private KeyboardShortcutAction _sut;
    private ActionConfig _config;

    public KeyboardShortcutActionTest()
    {
        _keyboardSimulatorMock = new Mock<IKeyboardSimulator>();
    }

    [Fact]
    public void Execute_ShouldCallSimulator_WithCorrectKeys()
    {
        // Arrange
        _config = new ActionConfig()
        {
            Parameters = new Dictionary<string, string>()
            {
                {"keys", "17, 65"}
            }
        };
        _sut = new KeyboardShortcutAction(_keyboardSimulatorMock.Object, _config);
        
        // Act
        _sut.Execute();
        
        // Assert
        _keyboardSimulatorMock.Verify(s => s.SendShortcut(It.Is<IEnumerable<int>>(
            keys => keys.SequenceEqual(new List<int>{17, 65})
        )), Times.Once);
    }
}