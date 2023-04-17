using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoadGap.webapi.Dtos;
using RoadGap.webapi.Models;
using RoadGap.webapi.Repositories;

namespace RoadGap.webapi.Controllers;

[ApiController]
[Route("tasks")]
public class TaskController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public TaskController(ITaskRepository taskRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Get([FromQuery] string? searchParam = null)
    {

        var tasks = searchParam is null
            ? _taskRepository.GetTasks()
            : _taskRepository.GetTasksBySearch(searchParam);
        ;
        return Ok(tasks);
    }

    [HttpGet("{taskId:int}")]
    public IActionResult Get(int taskId)
    {
        var task = _taskRepository.GetTaskById(taskId);
        if (task == null)
        {
            return NotFound("There's no task with this id.");
        }

        return Ok(task);
    }
    
    [HttpPut("{taskId:int}")]
    public IActionResult Edit(int taskId, [FromBody] TaskToUpsertDto taskDto)
    {
        var result = _taskRepository.EditTask(taskId, taskDto);

        if (result.Success) return Ok(result.Message);
        if (result.StatusCode == 404)
        {
            return NotFound(result.Message);
        }

        return BadRequest(result.Message);

    }

    [HttpPost]
    public IActionResult Create(TaskToUpsertDto taskToAdd)
    {
        // if (!_entityChecker.CategoryExists(taskToAdd.CategoryId))
        // {
        //     return BadRequest("Invalid category id.");
        // }
        //
        // if (!_entityChecker.StatusExists(taskToAdd.StatusId))
        // {
        //     return BadRequest("Invalid status id.");
        // }

        var task = _mapper.Map<TaskModel>(taskToAdd);

        _taskRepository.AddEntity(task);
        _taskRepository.SaveChanges();

        return Ok("Task created successfully.");
    }

    [HttpDelete("{taskId:int}")]
    public IActionResult Delete(int taskId)
    {
        var task = _taskRepository.GetTaskById(taskId);
        
        if (task == null)
        {
            return NotFound("There's no task with this id.");
        }

        _taskRepository.RemoveEntity(task);
        _taskRepository.SaveChanges();
        
        return Ok("Task deleted successfully.");
    }
}