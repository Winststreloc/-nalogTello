namespace AnalogTrello.Windows.App.Configuration;

public class AppConfig : IConfiguration
{
    public string AppName { get; set; }
    public string AppVersion { get; set; }
    public string AppExePath { get; set; }
    public string AppLogFolder { get; set; }
}