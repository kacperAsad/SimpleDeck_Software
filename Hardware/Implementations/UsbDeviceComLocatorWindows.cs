using System.Management;
using System.Text.RegularExpressions;
using Hardware.Interfaces;

namespace Hardware;

public class UsbDeviceComLocatorWindows : IUsbDeviceLocator
{

    public List<IUsbDeviceLocator.UsbDevice> LocateDevices()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption LIKE '%(COM%'");
        
        var allDevices = new List<IUsbDeviceLocator.UsbDevice>();

        foreach (var device in searcher.Get())
        {
            if (device == null) continue;
            
            string? name = device["Caption"].ToString();
            string? deviceId = device["DeviceID"].ToString();
            
            if (deviceId == null || name == null) continue;

            Match vidPidMatch = Regex.Match(deviceId, "VID_([0-9A-F]{4})&PID_([0-9A-F]{4})");
            Match portMatch = Regex.Match(name, @"\(COM(\d+)\)");

            if (!vidPidMatch.Success || !portMatch.Success) continue;
            
            string vid = vidPidMatch.Groups[1].Value;
            string pid = vidPidMatch.Groups[2].Value;
                
            string port = "COM" + portMatch.Groups[1].Value;
            
            allDevices.Add(new IUsbDeviceLocator.UsbDevice()
            {
                Port = port,
                VID = vid,
                PID = pid
            });
        }
        
        return allDevices;
    }
    
    public IUsbDeviceLocator.UsbDevice? LocateDevice(string vid, string pid)
    {
        return LocateDevices().FirstOrDefault(x => x.VID == vid && x.PID == pid);
    }
}