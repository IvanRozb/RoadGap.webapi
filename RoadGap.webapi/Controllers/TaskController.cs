using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoadGap.webapi.Dtos;
using RoadGap.webapi.Service;
using Task = RoadGap.webapi.Models.Task;

namespace RoadGap.webapi.Controllers;

[ApiController]
[Route("tasks")]
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
    
    [HttpGet]
    public IActionResult Get()
    {
        var tasks = _taskService.GetTasks();
        return Ok(tasks);
    }

    [HttpGet("{taskId:int}")]
    public IActionResult Get(int taskId)
    {
        var task = _taskService.GetTaskById(taskId);
        if (task == null)
        {
            return NotFound("There's no task with this id.");
        }

        return Ok(task);
    }

    [HttpGet("{searchParam}")]
    public IActionResult Get(string searchParam)
    {
        var searchedTasks = _taskService
            .GetTasksBySearch(searchParam);
        
        return Ok(searchedTasks);
    }
    
    [HttpPut("{taskId:int}")]
    public IActionResult Edit(int taskId, [FromBody] TaskToUpsertDto taskDto)
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

        if (taskDb == null)
        {
            return NotFound("There's no task with this id.");
        }

        _mapper.Map(taskDto, taskDb);

        _taskService.SaveChanges();

        return Ok("Task updated successfully.");
    }

    [HttpPost]
    public IActionResult Create(TaskToUpsertDto taskToAdd)
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

    [HttpDelete("{taskId:int}")]
    public IActionResult Delete(int taskId)
    {
        var task = _taskService.GetTaskById(taskId);
        
        if (task == null)
        {
            return NotFound("There's no task with this id.");
        }

        _taskService.RemoveEntity(task);
        _taskService.SaveChanges();
        
        return Ok("Task deleted successfully.");
    }
}