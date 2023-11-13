using System.ComponentModel.DataAnnotations;

namespace AnalogTrello.Models;

public class BaseEntity
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
}