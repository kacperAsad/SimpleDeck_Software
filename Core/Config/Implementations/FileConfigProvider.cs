using System.Text.Json;
using Core.Input;

namespace Core.Config.Implementations;

public class FileConfigProvider : IConfigProvider
{
    private string _filePath;
    
    private readonly string _fileName;
    private readonly string _folderName;

    public FileConfigProvider(string folderName, string fileName)
    {
        _fileName = fileName;
        _folderName = folderName;
    }

    public void Init()
    {
        var dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            _folderName);
        // Todo tutaj raczej nie taki string, prędzej globalny const czy coś
        Directory.CreateDirectory(dir);
        _filePath = Path.Combine(dir, _fileName);
    }
    
    public AppConfig Load()
    {
        if (_filePath == null)
        {
            Init();
        }
        if (!File.Exists(_filePath))
        {
            var defaultConfig = CreateDefault();
            Save(defaultConfig);
            return defaultConfig;
        }
        var json = File.ReadAllText(_filePath);

        return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();

    }

    public void Save(AppConfig config)
    {
        if (_filePath == null)
        {
            Init();
        }
        
        var json = JsonSerializer.Serialize(config, 
            new JsonSerializerOptions{WriteIndented = true});
        File.WriteAllText(_filePath, json);
    }

    private AppConfig CreateDefault()
    {
        return new AppConfig()
        {
            Mappings =
            {
                new ControlMappingConfig()
                {
                    Control = "VOL1",
                    Target = PotentiometerTarget.Master,
                    CurveType = "Log"
                },
                new ControlMappingConfig()
                {
                    Control = "VOL2",
                    Target = PotentiometerTarget.Application,
                    Process = "Youtube Music",
                    CurveType = "Log"
                },
                new ControlMappingConfig()
                {
                    Control = "VOL3",
                    Target = PotentiometerTarget.Application,
                    Process = "firefox",
                    CurveType = "Log"
                },
                new ControlMappingConfig()
                {
                    Control = "VOL4",
                    Target = PotentiometerTarget.Group,
                    GroupName = "Games",
                    CurveType = "Log"
                },
            },
            Buttons =
            {
                new ButtonMappingConfig()
                {
                    ControlId = "BTN1",
                    Action = new ActionConfig()
                    {
                        Type = ActionType.PlayPause,
                        Parameters =  new Dictionary<string, string>()
                    }
                },
                new ButtonMappingConfig()
                {
                    ControlId = "BTN2",
                    Action = new ActionConfig()
                    {
                        Type = ActionType.PreviousTrack,
                        Parameters =  new Dictionary<string, string>()
                    }
                },
                new ButtonMappingConfig()
                {
                    ControlId = "BTN3",
                    Action = new ActionConfig()
                    {
                        Type = ActionType.NextTrack,
                        Parameters =  new Dictionary<string, string>()
                    }
                    
                },
                new ButtonMappingConfig()
                {
                    ControlId = "BTN4",
                    Action = new ActionConfig()
                    {
                        Type = ActionType.OpenApp,
                        Parameters =  new Dictionary<string, string>()
                        {
                            { "path", "calc.exe" }
                        }
                    }
                }
                
            },
            AppGroups =
            {
                new AppGroupConfig()
                {
                    Name = "Games",
                    Processes =
                    [
                        "minecraft",
                        "factorio"
                    ]
                }
            }
        };
    }
}