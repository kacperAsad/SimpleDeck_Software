using Audio;
using Core.Config.Implementations;
using Core.Input;
using Core.Interfaces;
using Core.Routing;
using Hardware;
using Hardware.Implementations;
using Hardware.Interfaces;
using Infrastructure;


class Program
{
    const string VID = "0483";
    const string PID = "5740";
    
    
    static async Task Main(string[] args)
    {
        var configProvider = new FileConfigProvider("SimpleDeck");
        var config = configProvider.Load();

        // TUTAJ: Wzbogać mappings o AudioCurve obiekty
        var runtimeMappings = config.Mappings
            .Select(ControlMappingFactory.Create)
            .ToList();
        var runtimeButtonActions = config.Buttons
            .Select(ButtonMappingRuntimeFactory.Create)
            .ToList();
            
        var audio = new WindowsAudioService();
        var router = new InputRouter(audio, runtimeMappings, runtimeButtonActions);
        
        
        IDeviceConnection device = new SerialDeviceConnection(new UsbDeviceComLocatorWindows(), new SimpleDeckV1Parser());

        IUsbDeviceWatcher usbDeviceWatcher = new UsbDeviceWatcher(VID, PID);

        DeviceConnectionManager manager = new DeviceConnectionManager(usbDeviceWatcher, device);
        
        manager.Start();
        
        

        device.DeviceMessageReceived += (s, msg) =>
        {
            Console.WriteLine($"[{msg.Type}] = {msg.Value}");
        };

        device.DeviceMessageReceived += (s, msg) => router.Handle(msg);

        await device.ConnectAsync();

        Console.WriteLine("Good device running, press enter to exit");
        Console.ReadLine();

        await device.DisconnectAsync();

        // Cleanup
        if (audio is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}