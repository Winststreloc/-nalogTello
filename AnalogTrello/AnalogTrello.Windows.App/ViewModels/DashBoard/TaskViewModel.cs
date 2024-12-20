using AnalogTrello.Windows.App.Utilies;

namespace AnalogTrello.Windows.App.ViewModels;

public class TaskViewModel : BaseViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
}