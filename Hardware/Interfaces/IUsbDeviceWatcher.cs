namespace Hardware.Interfaces;

public interface IUsbDeviceWatcher : IDisposable
{
    event EventHandler DeviceConnected;
    event EventHandler DeviceDisconnected;

    void Start();
    void Stop();

}