using Core;

namespace Hardware.Interfaces;

public interface IDeviceProtocolParser
{
    public IEnumerable<DeviceMessage> Parse(string line);
}