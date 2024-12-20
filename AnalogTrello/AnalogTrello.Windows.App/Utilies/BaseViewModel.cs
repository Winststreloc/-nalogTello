using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AnalogTrello.Windows.App.Utilies;

public class BaseViewModel : INotifyPropertyChanged
{
    private readonly Dictionary<string, object> _properties = new();

    /// <summary>
    /// Gets the value of a property
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    protected T Get<T>([CallerMemberName] string name = null)
    {
        Debug.Assert(name != null, "name != null");
        object value = null;
        if (_properties.TryGetValue(name, out value))
            return value == null ? default(T) : (T)value;
        return default(T);
    }

    /// <summary>
    /// Sets the value of a property
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <remarks>Use this overload when implicitly naming the property</remarks>
    protected virtual void Set<T>(T value, [CallerMemberName] string name = null)
    {
        Debug.Assert(name != null, "name != null");
        if (Equals(value, Get<T>(name)))
            return;
        _properties[name] = value;
        OnPropertyChanged(name);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void Log(Exception ex)
    {
        MessageBox.Show(ex.Message);
    }

    public string Log(string message)
    {
        MessageBox.Show(message);
        return message;
    }

    public void OnPropertiesChanged(params string[] properties)
    {
        foreach (var property in properties)
        {
            OnPropertyChanged(property);
        }
    }

    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void FillObservableCollections<T>(ObservableCollection<T> target, IEnumerable<T> source)
    {
        target.Clear();

        if (source == null)
        {
            return;
        }

        foreach (var item in source)
        {
            target.Add(item);
        }
    }
}