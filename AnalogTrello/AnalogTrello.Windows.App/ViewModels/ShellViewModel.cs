using AnalogTrello.Windows.App.Configuration;
using Caliburn.Micro;

namespace AnalogTrello.Windows.App.ViewModels;

public class ShellViewModel : Conductor<object>
{
    private readonly IEventAggregator _eventAggregator;
    private readonly IUserConfig _userConfig;

    public ShellViewModel(IEventAggregator eventAggregator, 
        IUserConfig userConfig)
    {
        _eventAggregator = eventAggregator;
        _userConfig = userConfig;
    }

    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        await base.OnInitializedAsync(cancellationToken);

        if (!_userConfig.IsLoggedIn)
        {
            await _eventAggregator.PublishOnBackgroundThreadAsync(typeof(LoginViewModel), cancellationToken);
        }
    }

    public async Task OnNavigate(Type viewModel)
    {
        await _eventAggregator.PublishOnBackgroundThreadAsync(viewModel);
    }
}