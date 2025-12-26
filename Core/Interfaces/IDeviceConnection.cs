namespace Core.Interfaces;

public interface IDeviceConnection : IDisposable
{
    event EventHandler<DeviceMessage> DeviceMessageReceived;
    
    bool IsConnected { get; }
    
    Task ConnectAsync();
    Task DisconnectAsync();
    
    Task SendAsync(DeviceCommand message);
}