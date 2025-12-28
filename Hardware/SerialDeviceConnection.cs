using Core;
using Core.Interfaces;
using System.IO.Ports;
using System.Text;
using System.Management;
using System.Text.RegularExpressions;
using Hardware.Interfaces;

namespace Hardware;

public class SerialDeviceConnection
    : IDeviceConnection
{
    public event EventHandler<DeviceMessage>? DeviceMessageReceived;
    public bool IsConnected => _serialPort?.IsOpen ?? false;
    
    private readonly IUsbDeviceLocator deviceLocator;
    private readonly IDeviceProtocolParser parser;

    private SerialPort? _serialPort;
    private readonly StringBuilder _buffer = new();


    private readonly string __vid = "0483";
    private readonly string __pid = "5740"; // TODO Trzeba tu machąć jakieś wczytywanie albo coś


    public SerialDeviceConnection(IUsbDeviceLocator deviceLocator, IDeviceProtocolParser parser)
    {
        this.deviceLocator = deviceLocator; 
        this.parser = parser;
  
    }
    

    private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        if (!IsConnected) return;
        try
        {
            var data = _serialPort?.ReadExisting();
            _buffer.Append(data);

            while (TryReadLine(out var line))
            {
                var messages = parser.Parse(line);

                foreach (var message in messages)
                {
                    DeviceMessageReceived?.Invoke(this, message);
                }
            }
            
        } catch (Exception ex)
        {
            Console.WriteLine(ex);
            // Todo dodać normalny log z tego
        }
    }
    
    private bool TryReadLine(out string line)
    {
        var text = _buffer.ToString();
        var index = text.IndexOf('\n');

        if (index < 0)
        {
            line = "";
            return false;
        }

        line = text[..index].Trim();
        _buffer.Remove(0, index + 1);
        return true;
    }

    public Task ConnectAsync()
    {
        var portName = deviceLocator.LocateDevice(__vid, __pid)?.Port;

        if (portName == null) return Task.CompletedTask;
        
        _serialPort = new SerialPort(portName, 9600)
        {
            NewLine = "\n",
            Encoding = Encoding.ASCII
        };
        
        _serialPort.DataReceived += OnDataReceived;
        _serialPort.ErrorReceived += OnErrorReceived;
        
        _serialPort.Open();
        return Task.CompletedTask;
    }

    
    public Task DisconnectAsync()
    {
        _serialPort?.DataReceived -= OnDataReceived;
        _serialPort?.Close();
        return Task.CompletedTask;
    }
    
    public Task SendAsync(DeviceCommand command)
    {
        return Task.Run(() =>
        {
            if (_serialPort is { IsOpen: false } ) throw new InvalidOperationException("Port is not open");
            _serialPort?.WriteLine($"{command.Command}:{command.Value}");
            // Todo Tutaj tzeba zrobić jakieś op wysyłanie, i żeby simpledeck to przyjmował
        });
    }

    
    
    private void OnErrorReceived(object sender, SerialErrorReceivedEventArgs e)
    {
        if (e.EventType is SerialError.RXOver or SerialError.Frame)
        {
            DisconnectAsync();
        }
    }
    
    
    
    public void Dispose()
    {
        if (_serialPort is { IsOpen: true })
        {
            _serialPort.Close();
        }
        _serialPort?.DataReceived -= OnDataReceived;
        _serialPort?.Dispose();
        _buffer.Clear();
    }
    
    
    
    
    
}