namespace AnalogTrello.Models;

public class User : BaseEntity
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public ICollection<Task> Tasks { get; set; }
}