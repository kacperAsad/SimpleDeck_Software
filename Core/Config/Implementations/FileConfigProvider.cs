using System.Text.Json;
using Core.Input;

namespace Core.Config.Implementations;

public class FileConfigProvider : IConfigProvider
{
    private readonly string _filePath;

    public FileConfigProvider(string filePath)
    {
        var dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SimpleDeck");
        // Todo tutaj raczej nie taki string, prędzej globalny const czy coś
        Directory.CreateDirectory(dir);
        _filePath = Path.Combine(dir, "config.json");
    }
    
    
    public AppConfig Load()
    {
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
                    Target = "Master",
                    CurveType = "Log"
                },
                new ControlMappingConfig()
                {
                    Control = "VOL2",
                    Target = "Application",
                    CurveType = "Log"
                },
                new ControlMappingConfig()
                {
                    Control = "VOL3",
                    Target = "Application",
                    CurveType = "Log"
                },
                new ControlMappingConfig()
                {
                    Control = "VOL4",
                    Target = "Application",
                    CurveType = "Log"
                },
            }
        };
    }
}