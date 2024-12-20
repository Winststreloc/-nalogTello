namespace AnalogTrello.Windows.App.Configuration;

public interface IConfiguration
{
    string AppName { get; set; }
    string AppVersion { get; set; }
    string AppExePath { get; set; }
    string AppLogFolder { get; set; }
}