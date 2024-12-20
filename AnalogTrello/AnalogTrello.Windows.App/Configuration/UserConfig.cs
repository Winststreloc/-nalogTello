using AnalogTrello.Windows.App.Utilies;

namespace AnalogTrello.Windows.App.Configuration;

public class UserConfig : BaseViewModel, IUserConfig
{
    public bool IsLoggedIn
    {
        get => Get<bool>();
        set => Set(value);
    }
}