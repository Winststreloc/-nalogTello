using System.Collections.ObjectModel;
using AnalogTrello.Windows.App.Utilies;

namespace AnalogTrello.Windows.App.ViewModels.DashBoard;

public class DashBoardViewModel : BaseViewModel
{
    public ObservableCollection<TaskViewModel> ToDoTasks { get; set; }
    public ObservableCollection<TaskViewModel> InProgressTasks { get; set; }
    public ObservableCollection<TaskViewModel> DoneTasks { get; set; }

    public DashBoardViewModel()
    {
        ToDoTasks = new ObservableCollection<TaskViewModel>
        {
            new TaskViewModel { Title = "Task 1", Description = "Description for task 1" },
            new TaskViewModel { Title = "Task 2", Description = "Description for task 2" }
        };

        InProgressTasks = new ObservableCollection<TaskViewModel>();
        DoneTasks = new ObservableCollection<TaskViewModel>();
    }
}