using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoadGap.webapi.Data;
using RoadGap.webapi.Dtos;
using RoadGap.webapi.Service;
using RoadGap.webapi.Service.Implementation;
using Task = RoadGap.webapi.Models.Task;

namespace RoadGap.webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
        _mapper = new Mapper(new MapperConfiguration(configurationExpression =>
        {
            configurationExpression.CreateMap<TaskToUpsertDto, Task>();
        }));
    }
    
    [HttpGet("GetTasks")]
    public IActionResult GetTasks()
    {
        var tasks = _taskService.GetTasks();
        return Ok(tasks);
    }

    [HttpGet("GetTaskById/{taskId:int}")]
    public IActionResult GetTaskById(int taskId)
    {
        var task = _taskService.GetTaskById(taskId);
        return Ok(task);
    }

    [HttpGet("GetTasksBySearch/{searchParam}")]
    public IActionResult GetTasksBySearch(string searchParam)
    {
        var searchedTasks = _taskService
            .GetTasksBySearch(searchParam);
        
        return Ok(searchedTasks);
    }
    
    [HttpPut("EditTask/{taskId:int}")]
    public IActionResult EditTask(int taskId, [FromBody] TaskToUpsertDto taskDto)
    {
        if (!_taskService.CategoryExists(taskDto.CategoryId))
        {
            return BadRequest("Invalid category id.");
        }

        if (!_taskService.StatusExists(taskDto.StatusId))
        {
            return BadRequest("Invalid status id.");
        }

        var taskDb = _taskService.GetTaskById(taskId);

        _mapper.Map(taskDto, taskDb);

        _taskService.SaveChanges();

        return Ok("Task updated successfully.");
    }

    [HttpPost("CreateTask")]
    public IActionResult CreateTask(TaskToUpsertDto taskToAdd)
    {
        if (!_taskService.CategoryExists(taskToAdd.CategoryId))
        {
            return BadRequest("Invalid category id.");
        }

        if (!_taskService.StatusExists(taskToAdd.StatusId))
        {
            return BadRequest("Invalid status id.");
        }
        
        var task = _mapper.Map<Task>(taskToAdd);

        _taskService.AddEntity(task);
        
        _taskService.SaveChanges();
        
        return Ok("Task created successfully.");
    }

    [HttpDelete("DeleteTask/{taskId:int}")]
    public IActionResult DeleteTask(int taskId)
    {
        var task = _taskService.GetTaskById(taskId);

        _taskService.RemoveEntity(task);
        _taskService.SaveChanges();
        
        return Ok("Task deleted successfully.");
    }
}