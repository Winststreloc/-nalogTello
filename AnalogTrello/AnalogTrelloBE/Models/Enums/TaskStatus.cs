using System.ComponentModel.DataAnnotations;

namespace AnalogTrelloBE.Models.Enums;

public enum TaskStatus
{
    [Display(Name = "Начата")]
    Started = 1,
    [Display(Name = "В процессе")]
    InProgress = 2,
    [Display(Name = "Завершена")]
    Ended = 3,
}