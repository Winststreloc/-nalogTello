using TaskStatus = AnalogTrelloBE.Models.Enums.TaskStatus;

namespace AnalogTrelloBE.Models;

public class TaskScheduler : BaseEntity
{
    public string Title { get; set; }
    public string Text { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
    public TaskStatus TaskStatus { get; set; }
    public DateTime EndTimeTask { get; set; }
}