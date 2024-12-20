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
        return await context.Tasks.ToArrayAsync();
    }

    public async Task<TaskScheduler?> GetTask(long id)
    {
        return await context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<TaskSchedulerDto> CreateTask(TaskSchedulerDto taskDto)
    {
        var task = mapper.Map<TaskScheduler>(taskDto);

        context.Tasks.Add(task);

        await context.SaveChangesAsync();
        
        var result = mapper.Map<TaskSchedulerDto>(task);

        return result;
    }
    

    public async Task DeleteTasks(IEnumerable<long> ids)
    {
        var deleteTasks = await context.Tasks
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();

        context.Tasks.RemoveRange(deleteTasks);

        await context.SaveChangesAsync();
    }

    public async Task DeleteTask(long id)
    {
        var task = await context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

        if (task == null)
        {
            return;
        }
        
        context.Tasks.Remove(task);

        await context.SaveChangesAsync();
    }

    public async Task UpdateTasks(TaskSchedulerDto[] tasks)
    {
        var ids = tasks
            .Select(dto => dto.Id)
            .ToList();
        
        var existingTasks = await context.Tasks
            .Where(t => ids.Contains(t.Id))
            .ToListAsync();

        foreach (var taskScheduler in existingTasks)
        {
            var dto = tasks.FirstOrDefault(t => t.Id == taskScheduler.Id);
            
            if (dto == null)
            {
                continue;
            }
            
            taskScheduler.Title = dto.Title;
            taskScheduler.Text = dto.Text;
            taskScheduler.EndTimeTask = dto.EndTimeTask;
            taskScheduler.TaskStatus = dto.TaskStatus;
        }

        await context.SaveChangesAsync();
    }

    public async Task UpdateTask(TaskSchedulerDto taskDto)
    {
        var task = await context.Tasks.FirstOrDefaultAsync(x => x.Id == taskDto.Id);

        if (task == null)
        {
            return;
        }
        
        task.Title = taskDto.Title;
        task.Text = taskDto.Text;
        task.EndTimeTask = taskDto.EndTimeTask;
        task.TaskStatus = taskDto.TaskStatus;

        await context.SaveChangesAsync();
    }
}