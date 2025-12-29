using Core;
using Hardware.Interfaces;

namespace Hardware;

public class SimpleDeckV1Parser : IDeviceProtocolParser
{
    
    private int[] _lastData = new int[8];
    
    public IEnumerable<DeviceMessage> Parse(string line)
    {
        if (line.StartsWith("-"))
        {
            yield break;
        } // Command, not data
        
        var splitted = line.Split(':');

        if (splitted.Length != 8) // Here only for simple deck v1. Need to think of a better way TODO
        {
            yield break;
        }
        
        int[] integerValues = new int[8];
        
        try
        {
            integerValues = splitted.Select(int.Parse).ToArray();
        }
        catch (Exception e)
        {
            yield break;
            // ignored
        }

        for (int i = 0; i < 4; i++)
        {
            int difference = Math.Abs(integerValues[i] - _lastData[i]);

            if (difference > 1 || integerValues[i] == 0 )
            {
                _lastData[i] = integerValues[i];
                yield return new DeviceMessage($"VOL{i + 1}", (100 - integerValues[i]));
            }
        }

        for (int i = 4; i < 8; i++)
        {
            if (integerValues[i] != _lastData[i])
            {
                _lastData[i] = integerValues[i];
                yield return new DeviceMessage($"BTN{i - 3}", integerValues[i]);
            }
        }
    
        
    }
}