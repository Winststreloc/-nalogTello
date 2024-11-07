using AnalogTrello.Models;

namespace AnalogTrelloBE.Models;

public class Task : BaseEntity
{
    public string Title { get; set; }
    public string Text { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
    public TaskStatus TaskStatus { get; set; }
    public DateTime EndTimeTask { get; set; }
}