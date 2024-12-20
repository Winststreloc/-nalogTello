using AnalogTrello.Windows.App.ViewModels;
using AnalogTrello.Windows.App.Views;
using Autofac;

namespace AnalogTrello.Windows.App.Ioc;

public class UiModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        
        builder.RegisterType<ShellView>()
            .OnActivated(c => c.Instance.DataContext = c.Context.Resolve<ShellViewModel>())
            .AsImplementedInterfaces()
            .AsSelf()
            .SingleInstance();
        
        builder.RegisterAssemblyTypes(typeof(App).Assembly)
            .Where(t => t.Name.EndsWith("ViewModel") && t.GetConstructors().Any())
            .AsImplementedInterfaces()
            .AsSelf()
            .SingleInstance();
    }
}