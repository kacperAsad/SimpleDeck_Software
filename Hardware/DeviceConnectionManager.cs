using System.Management;
using Core.Interfaces;
using Hardware.Interfaces;

namespace Hardware;


/// <summary>
/// Class that manage Serial Connection - Detect when it was dissconected, and detect connection.
/// TODO: Dodać możliwość podłączenia kilku urządzeń
/// </summary>

public class DeviceConnectionManager : IDisposable
{
    public event EventHandler<bool>? StateChanged; 
    
    
    private readonly IUsbDeviceWatcher watcher;
    private readonly IDeviceConnection deviceConnection;

    public bool IsDeviceConnected => deviceConnection.IsConnected;
    


    public DeviceConnectionManager(IUsbDeviceWatcher watcher, IDeviceConnection deviceConnection)
    {
        this.watcher = watcher;
        this.deviceConnection = deviceConnection;

        watcher.DeviceConnected += OnDeviceConnected;
        watcher.DeviceDisconnected += OnDeviceDisconnected;
    }


    public void Start()
    {
        watcher.Start();
    }
    
    private async void OnDeviceConnected(object? sender, EventArgs e)
    {
        if (!deviceConnection.IsConnected)
            await deviceConnection.ConnectAsync();
        if (IsDeviceConnected)
            StateChanged?.Invoke(this, true); // TODO to powinno wychodzić z SerialDeviceConnection

    }

    private async void OnDeviceDisconnected(object? sender, EventArgs e)
    {
        if (deviceConnection.IsConnected)
            await deviceConnection.DisconnectAsync();
        if (!IsDeviceConnected)
            StateChanged?.Invoke(this, false); // TODO: to powinno wychodzić z SerialDeviceConnection
        
        
    }



    public void Dispose()
    {
        watcher.DeviceConnected -= OnDeviceConnected;
        watcher.DeviceDisconnected -= OnDeviceDisconnected;
        watcher.Dispose();
        deviceConnection.Dispose();
    }
}