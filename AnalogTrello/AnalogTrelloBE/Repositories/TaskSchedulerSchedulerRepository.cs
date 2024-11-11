using AnalogTrelloBE.Data;
using AnalogTrelloBE.Dto;
using AnalogTrelloBE.Interfaces.IRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskScheduler = AnalogTrelloBE.Models.TaskScheduler;

namespace AnalogTrelloBE.Repositories;

public class TaskSchedulerSchedulerRepository(ToDoDbContext context,
    IMapper mapper) : ITaskSchedulerRepository
{
    public async Task<TaskScheduler[]> GetAllTasks()
    {
        throw new NotImplementedException();
    }

    public async Task<TaskScheduler?> GetTask(long id)
    {
        return await context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
    }

    public TaskSchedulerDto CreateTask(TaskSchedulerDto taskDto)
    {
        var task = mapper.Map<TaskScheduler>(taskDto);

        context.Tasks.Add(task);

        var result = mapper.Map<TaskSchedulerDto>(task);

        return result;
    }

    public async Task DeleteTasks(IEnumerable<long> ids)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteTask(long id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateTasks(IEnumerable<TaskSchedulerDto> tasks)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateTask(TaskSchedulerDto taskSchedulerDto)
    {
        throw new NotImplementedException();
    }
}