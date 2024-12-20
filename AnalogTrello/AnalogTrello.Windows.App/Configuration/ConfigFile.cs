using System.Reflection;

namespace AnalogTrello.Windows.App.Configuration;

public class ConfigFile : IStorageFile
{
    public string Path()
    {
        return System.IO.Path.Combine(
            System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
            "App.config");
    }
}