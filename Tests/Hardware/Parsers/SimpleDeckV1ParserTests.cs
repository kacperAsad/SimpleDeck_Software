using System.Collections;
using Core;
using Hardware;
using Xunit.Abstractions;

namespace Tests.Parsers;

public class SimpleDeckV1ParserTests
{
    private readonly SimpleDeckV1Parser _parser;
    
    public SimpleDeckV1ParserTests()
    {
        _parser = new SimpleDeckV1Parser();
    }

    [Fact]
    public void Parse_ValidLine_ReturnsDeviceMessages()
    {
        string correctLine = "10:10:10:10:1:1:1:1";
        
        var result = _parser.Parse(correctLine);
        
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Parse_VolumeChange_ReturnsDeviceMessage()
    {
        
        // arrange
        string firstLine = "10:10:10:10:1:1:1:1";
        string secondLine = "20:10:20:10:1:0:1:0";
        
        var unusedResult = _parser.Parse(firstLine).ToList();
        
        
        
        var result = _parser.Parse(secondLine).ToList();

        Assert.NotEmpty(result);

        // Parser is mapping vol values from 100 - 0, to 0 - 100
        
        var msg1 = result[0];
        Assert.Equal("VOL1", msg1.Type);
        Assert.Equal(80, msg1.Value);
        
        var msg2 = result[1];
        Assert.Equal("VOL3", msg2.Type);
        Assert.Equal(80, msg2.Value);
        
        var msg3 = result[2];
        Assert.Equal("BTN2", msg3.Type);
        Assert.Equal(0, msg3.Value);
        
        var msg4 = result[3];
        Assert.Equal("BTN4", msg4.Type);
        Assert.Equal(0, msg4.Value);
    }
    
    [Fact]
    public void Parse_NoChange_ReturnsEmptyList()
    {
        string firstLine = "10:10:10:10:1:1:1:1";
        
        var unusedResult = _parser.Parse(firstLine).ToList();
        
        var result = _parser.Parse(firstLine).ToList();
        
        Assert.Empty(result);
    }
    
    [Fact]
    public void Parse_SmallJitter_ReturnsEmptyList()
    {
        string firstLine = "10:10:10:10:1:1:1:1";
        
        var unusedResult = _parser.Parse(firstLine).ToList();
        
        var result = _parser.Parse("11:10:10:10:1:1:1:1").ToList();
        
        Assert.Empty(result);
    }
    
    [Fact]
    public void Parse_ButtonChange_ReturnsDeviceMessage()
    {
        string firstLine = "10:10:10:10:1:1:1:1";
        
        var usuedResult = _parser.Parse(firstLine).ToList();
        
        var result = _parser.Parse("10:10:10:10:0:1:1:1").ToList();
        
        Assert.Single(result);
        
    }

    [Theory]
    [InlineData("a:b:c:d:e:f:g:h")]
    [InlineData("alfa")]
    [InlineData("test1:test2")]
    [InlineData("12:test3:test4")]
    [InlineData("12:test4:test5:12")]
    [InlineData("12:12:12:12:1:1:1:k")]
    [InlineData("12:12:12:12:12:1:1:1;")]
    [InlineData("12:12:12:12:12:1;1:1")]
    public void Parse_InvalidData_ReturnsEmptyList(string data)
    {
        string firstLine = data;
        
        var result = _parser.Parse(firstLine).ToList();
        
        Assert.Empty(result);
    }
    
    [Fact]
    public void Parse_FieldCountMismatch_ReturnsEmptyList()
    {
        string firstLine = "10:10";
        
        var result = _parser.Parse(firstLine).ToList();
        
        Assert.Empty(result);
    }
}