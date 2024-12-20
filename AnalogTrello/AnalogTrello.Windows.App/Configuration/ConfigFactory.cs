namespace AnalogTrello.Windows.App.Configuration;

public class ConfigFactory
{
    private static IConfiguration? _config;

    public IConfiguration? Config(bool savingAllowed = false)
    {

        return _config ??= InitializedConfig();
    }

    private static IConfiguration? InitializedConfig()
    {
        var config = new DefaultConfig(new ConfigFile());

        return config.Value();
    }
}