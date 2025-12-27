using System.Management;
using Hardware.Interfaces;

namespace Hardware.Implementations;


// Powinno brać objekt SerialDeviceConnection, i go connecttować / disconnectować i zarządzać nim. 

public class UsbDeviceWatcher (
    string __vid,
    string __pid
    ): IUsbDeviceWatcher
{
    

    public event EventHandler? DeviceConnected;
    public event EventHandler? DeviceDisconnected;
    
    private ManagementEventWatcher? insertWatcher;
    private ManagementEventWatcher? removeWatcher;
    
    
    
    
    public void Start()
    {
        insertWatcher = new ManagementEventWatcher(new WqlEventQuery(
            "SELECT * FROM __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_PnPEntity'"
        ));
        
        insertWatcher.EventArrived += (s, e) =>
        {
            Console.WriteLine("TEst czy działa event wgl");
            if (MatchesDevice(e))
                Console.WriteLine("Test czy wykryło device");
                DeviceConnected?.Invoke(this, EventArgs.Empty);
            
        };

        removeWatcher = new ManagementEventWatcher(
            new WqlEventQuery(
                "SELECT * FROM __InstanceDeletionEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_PnPEntity'"
            ));

        removeWatcher.EventArrived += (s, e) =>
        {
            if (MatchesDevice(e))
                
                DeviceDisconnected?.Invoke(this, EventArgs.Empty);
        };

        insertWatcher.Start();
        removeWatcher.Start();
        
        
    }

    public void Stop()
    {
        insertWatcher?.Stop();
        removeWatcher?.Stop();
    }

    private bool MatchesDevice(EventArrivedEventArgs eventArgs)
    {
        var instance = (ManagementBaseObject)eventArgs.NewEvent["TargetInstance"];
        var deviceId = instance["DeviceID"]?.ToString();

        if (deviceId == null) return false;

        return deviceId.Contains($"VID_{__vid}", StringComparison.OrdinalIgnoreCase)
               && deviceId.Contains($"PID_{__pid}", StringComparison.OrdinalIgnoreCase);
    }
    
    
    
    
    
    public void Dispose()
    {
        Stop();
        insertWatcher?.Dispose();
        removeWatcher?.Dispose();
    }
}