using Core.Audio;

namespace Tests.Audio;

public class VolumeCurveTest
{
    [Theory]
    [InlineData(0, 0)] // 0 in, 0 out
    [InlineData(100, 1.0)]
    [InlineData(50, 0.5)]
    public void LinearCurve_ShouldReturnCorrectValue(float input, float expected)
    {
        // Arrange
        var curve = new LinearVolumeCurve();
        
        // Act
        var result = curve.Map(input);
        
        // Assert
        Assert.Equal(expected, result, precision: 2);
    }

    [Fact]
    public void LogCurve_ShouldBeLowerThanLinearAtMidPoint()
    {
        // Arrange
        var logCurve = new LogVolumeCurve();
        
        // Act
        var result = logCurve.Map(50);
        
        // Assert 
        Assert.True(result < 0.5f);
    }
}