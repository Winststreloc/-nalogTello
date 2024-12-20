using AnalogTrello.Windows.App.Configuration;
using Autofac;
using Caliburn.Micro;

namespace AnalogTrello.Windows.App.Ioc;

public class AppModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);


        builder.RegisterType<UserConfig>().As<IUserConfig>().SingleInstance();
        builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
        builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();
    }
}