using AnalogTrelloBE.Dto;
using AnalogTrelloBE.Interfaces.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskScheduler = AnalogTrelloBE.Models.TaskScheduler;

namespace AnalogTrelloBE.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class TaskSchedulerController(ITaskSchedulerRepository taskSchedulerRepository) : ControllerBase
{
    [HttpGet("tasks")]
    public async Task<ResponseDto<TaskScheduler[]>> GetTasks()
    {
        var tasks = await taskSchedulerRepository.GetAllTasks();

        if (tasks.Length == 0)
        {
            return ResponseDto<TaskScheduler[]>.Success(tasks);
        }
        
        Response.StatusCode = 404;
        return ResponseDto<TaskScheduler[]>.Failed("Not found");

    }
    
    [HttpGet("task")]
    public async Task<ResponseDto<TaskScheduler>> GetTask([FromQuery]long id)
    {
        var task = await taskSchedulerRepository.GetTask(id);

        if (task != null)
        {
            return ResponseDto<TaskScheduler>.Success(task);
        }
        
        Response.StatusCode = 404;
        return ResponseDto<TaskScheduler>.Failed("Not found");

    }
    
    [HttpPost("task")]
    public ResponseDto<TaskSchedulerDto> CreateTask([FromBody]TaskSchedulerDto task)
    {
        var createdTask = taskSchedulerRepository.CreateTask(task);

        return ResponseDto<TaskSchedulerDto>.Success(createdTask);
    }
    
    [HttpPut("task")]
    public IActionResult UpdateTask([FromBody]TaskSchedulerDto task)
    {
        taskSchedulerRepository.UpdateTask(task);

        Response.StatusCode = 204;

        return Ok();
    }
}