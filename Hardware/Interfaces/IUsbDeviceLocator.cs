namespace Hardware.Interfaces;

public interface IUsbDeviceLocator
{
    public List<UsbDevice> LocateDevices();
    
    public UsbDevice? LocateDevice(string vid, string pid);

    public class UsbDevice
    {
        public string Port { get; set; }
        public string VID { get; set; }
        public string PID { get; set; }
    }
}