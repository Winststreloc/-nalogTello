using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Controls;

namespace AnalogTrello.Windows.App.Controls;

public class NavButton : ListBoxItem
{
    static NavButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NavButton), new FrameworkPropertyMetadata(typeof(NavButton)));
    }

    public Type Navlink
    {
        get => (Type)GetValue(NavlinkProperty);
        set => SetValue(NavlinkProperty, value);
    }

    public static readonly DependencyProperty NavlinkProperty =
        DependencyProperty.Register("Navlink", typeof(Type), typeof(NavButton), new PropertyMetadata(null));

    public ImageIcon Icon
    {
        get => (ImageIcon)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(ImageIcon), typeof(NavButton), new PropertyMetadata(null));
    
    public string MethodName
    {
        get => (string)GetValue(MethodNameProperty);
        set => SetValue(MethodNameProperty, value);
    }

    public static readonly DependencyProperty MethodNameProperty =
        DependencyProperty.Register("MethodName", typeof(string), typeof(NavButton), new PropertyMetadata(null));
    
    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);
        if (!string.IsNullOrEmpty(MethodName))
        {
            var viewModel = DataContext;
            var method = viewModel?.GetType().GetMethod(MethodName);

            if (method != null)
            {
                method.Invoke(viewModel, [Navlink]);
            }
        }
    }
}