using Audio;
using Core.Config.Implementations;
using Core.Input;
using Core.Interfaces;
using Core.Routing;
using Hardware;
using Infrastructure;


class Program
{
    static async Task Main(string[] args)
    {
        var configProvider = new FileConfigProvider("SimpleDeck");
        var config = configProvider.Load();

        // TUTAJ: Wzbogać mappings o AudioCurve obiekty
        var enrichedMappings = config.Mappings.Select(mapping =>
        {
            mapping.AudioCurve = VolumeCurveFactory.Create(mapping.CurveType);
            return mapping;
        }).ToList();


        IDeviceConnection device = new SerialDeviceConnection(new UsbDeviceComLocatorWindows(), new SimpleDeckV1Parser());

        // Opakowujemy WindowsAudioService w SmoothAudioService dla płynnych przejść
        var windowsAudio = new WindowsAudioService();
        var audio = new SmoothAudioService(windowsAudio);
        var router = new InputRouter(audio, enrichedMappings);

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