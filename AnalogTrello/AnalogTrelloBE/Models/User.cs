namespace AnalogTrelloBE.Models;

public class User : BaseEntity
{
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public ICollection<TaskScheduler> Tasks { get; set; }
}