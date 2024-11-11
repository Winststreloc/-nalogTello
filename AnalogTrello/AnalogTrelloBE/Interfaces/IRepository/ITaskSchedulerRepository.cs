using System.Collections;
using AnalogTrelloBE.Dto;
using TaskScheduler = AnalogTrelloBE.Models.TaskScheduler;

namespace AnalogTrelloBE.Interfaces.IRepository;

public interface ITaskSchedulerRepository
{
    Task<TaskScheduler[]> GetAllTasks();
    Task<TaskScheduler?> GetTask(long id);
    TaskSchedulerDto CreateTask(TaskSchedulerDto task);
    Task DeleteTasks(IEnumerable<long> ids);
    Task DeleteTask(long id);
    Task UpdateTasks(IEnumerable<TaskSchedulerDto> tasks);
    Task UpdateTask(TaskSchedulerDto taskSchedulerDto);
}