using System.Windows;
using AnalogTrello.Windows.App.Ioc;
using AnalogTrello.Windows.App.ViewModels;
using Autofac;
using Caliburn.Micro;

namespace AnalogTrello.Windows.App;

public class Bootstraper : BootstrapperBase
{
    private IContainer _container;
    
    private T Resolve<T>() => _container.Resolve<T>();
    
    public Bootstraper()
    {
        Initialize();
    }

    protected override void Configure()
    {
        base.Configure();

        ContainerBuilder builder = new();

        builder
            .RegisterModule<UiModule>()
            .RegisterModule<AppModule>();

        _container = builder.Build();
    }
    
    protected override void OnStartup(object sender, StartupEventArgs e)
    {
        DisplayRootViewForAsync<ShellViewModel>();
    }
}