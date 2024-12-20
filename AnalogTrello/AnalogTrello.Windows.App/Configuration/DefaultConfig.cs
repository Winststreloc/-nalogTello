using System.IO;
using Newtonsoft.Json;

namespace AnalogTrello.Windows.App.Configuration;

public class DefaultConfig
{
    private readonly IStorageFile _file;
    
    public DefaultConfig(IStorageFile file)
    {
        _file = file;
    }
    
    public IConfiguration? Value()
    {
        var serializer = GetSerializer();
        using StreamReader stream = new(_file.Path());
        using JsonTextReader reader = new(stream);
        return serializer.Deserialize<AppConfig>(reader);
    }

    private JsonSerializer GetSerializer()
    {
        return new JsonSerializer();
    }
}