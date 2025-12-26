using Core.Config;

namespace Core.Input;

public interface IConfigProvider
{
    AppConfig Load();
    void Save(AppConfig config);
}