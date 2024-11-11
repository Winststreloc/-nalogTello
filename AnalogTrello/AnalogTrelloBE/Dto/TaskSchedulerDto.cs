namespace AnalogTrelloBE.Dto;

public class TaskSchedulerDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public long UserId { get; set; }
    public TaskStatus TaskStatus { get; set; }
    public DateTime EndTimeTask { get; set; }
}