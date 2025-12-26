using Core;
using Core.Interfaces;
using Timer = System.Timers.Timer;

namespace Hardware.Test;

public class FakeDeviceConnection : IDeviceConnection 
{
    public event EventHandler<DeviceMessage>? DeviceMessageReceived;

    private readonly Timer _timer;
    private int _value;

    public FakeDeviceConnection()
    {
        _timer = new Timer(100);
        _timer.Elapsed += (_, _) => Generate();
    }


    public bool IsConnected { get; private set; }

    public Task ConnectAsync()
    {
        _timer.Start();
        IsConnected = true;
        return Task.CompletedTask;
    }

    public Task DisconnectAsync()
    {
        _timer.Stop();
        IsConnected = false;
        return Task.CompletedTask;
    }

    public Task SendAsync(DeviceCommand message)
    {
        return Task.CompletedTask;
    }

    private void Generate()
    {
        _value = (_value + 10) % 100;
        DeviceMessageReceived?.Invoke(this, 
            new DeviceMessage("VOL1", _value));
        DeviceMessageReceived?.Invoke(this, 
            new DeviceMessage("VOL2", _value));
        DeviceMessageReceived?.Invoke(this, 
            new DeviceMessage("VOL3", _value));
        DeviceMessageReceived?.Invoke(this, 
            new DeviceMessage("VOL4", _value));

        var rnd = new Random().Next(0, 2);
        
        DeviceMessageReceived?.Invoke(this, 
            new DeviceMessage("BTN1", rnd));
        DeviceMessageReceived?.Invoke(this, 
            new DeviceMessage("BTN2", rnd));
        DeviceMessageReceived?.Invoke(this, 
            new DeviceMessage("BTN3", rnd));
        DeviceMessageReceived?.Invoke(this, 
            new DeviceMessage("BTN4", rnd));
    }

    public void Dispose()
    {
        _timer.Dispose();
        
    }
}