using Core.Actions;
using Core.Config;
using Core.Interfaces;
using Moq;

namespace Tests.Core.Actions;

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
    
    [Fact]
    public void Execute_WhenNoKeysProvided_ShouldNotCallSimulator()
    {
        // Arrange
        _config = new ActionConfig();
        _sut = new KeyboardShortcutAction(_keyboardSimulatorMock.Object, _config);
        
        // Act
        _sut.Execute();
        
        // Assert
        _keyboardSimulatorMock.Verify(s => s.SendShortcut(It.IsAny<IEnumerable<int>>()), Times.Never);
    }
    
    [Theory]
    [InlineData("17,65", new int[] { 17, 65 })]
    [InlineData("  17 , 65  ", new int[] { 17, 65 })]
    [InlineData("1", new int[] { 1 })] 
    public void Initialize_ShouldParseDifferentFormatsCorrectly(string input, int[] expected)
    {
        // Arrange
        var config = new ActionConfig(){Parameters = new Dictionary<string, string>() {{"keys", input}}};
        _sut =  new KeyboardShortcutAction(_keyboardSimulatorMock.Object, config);
        
        // Act
        
        _sut.Execute();

        // Assert
        _keyboardSimulatorMock.Verify(s => s.SendShortcut(It.Is<IEnumerable<int>>(
            keys => keys.SequenceEqual(expected)
        )), Times.Once);
    }

    [Theory]
    [InlineData("17;65", new int[] {})]
    [InlineData("  17 , a  ", new int[] {})]
    [InlineData("A2,D",  new int[] {})]
    public void Initialize_ShouldParseIncorrectFormatWithoutAnyError(string input, int[] expected)
    {
        // Arrange
        var config = new ActionConfig()
        {
            Parameters = new Dictionary<string, string>()
            {
                { "keys", input }
            }
        };
        _sut = new KeyboardShortcutAction(_keyboardSimulatorMock.Object, config);
        
        // Act
        _sut.Execute();
        
        // Assert
        _keyboardSimulatorMock.Verify(s => s.SendShortcut(It.Is<IEnumerable<int>>(
            keys => keys.SequenceEqual(expected)
            )), Times.Never);
    }


    [Theory]
    [InlineData("17,65,42", new int[] { 17, 65, 42 })]
    [InlineData(" 1 , 2 , 2 , 3, 4", new int[] { 1, 2,2, 3, 4 })] // TODO nie wiem czy to git, żeby zwracało duplikat liczby, trzeba poczytać
    public void Execute_WhenManyKeysIsPressed_ShouldCallSimulator(string input, int[] expected)
    {
        // Arrange
        _config = new ActionConfig()
        {
            Parameters = new Dictionary<string, string>()
            {
                { "keys", input }
            }
        };
        _sut = new KeyboardShortcutAction(_keyboardSimulatorMock.Object, _config);
        
        // Act
        _sut.Execute();
        
        // Assert
        _keyboardSimulatorMock.Verify(s =>
            s.SendShortcut(It.Is<IEnumerable<int>>(keys => keys.SequenceEqual(expected)
            )), Times.Once);

    }
}