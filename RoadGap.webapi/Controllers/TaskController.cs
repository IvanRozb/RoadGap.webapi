using Microsoft.AspNetCore.Mvc;
using RoadGap.webapi.Dtos;
using RoadGap.webapi.Repositories;

namespace RoadGap.webapi.Controllers;

[ApiController]
[Route("tasks")]
public class TaskController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;

    public TaskController(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    [HttpGet]
    public IActionResult Get([FromQuery] string? searchParam = null)
    {
        var result = 
            _taskRepository.GetTasks(searchParam ?? "");
        return result.ToActionResult();
    }

    [HttpGet("{taskId:int}")]
    public IActionResult Get(int taskId)
    {
        var result = _taskRepository.GetTaskById(taskId);
        return result.ToActionResult();
    }
    
    [HttpPut("{taskId:int}")]
    public IActionResult Edit(int taskId, [FromBody] TaskToUpsertDto taskDto)
    {
        var result = _taskRepository.EditTask(taskId, taskDto);
        return result.ToActionResult();
    }

    [HttpPost]
    public IActionResult Create(TaskToUpsertDto taskToAdd)
    {
        var result = _taskRepository.CreateTask(taskToAdd);
        return result.ToActionResult();
    }

    [HttpDelete("{taskId:int}")]
    public IActionResult Delete(int taskId)
    {
        var result = _taskRepository.DeleteTask(taskId);
        return result.ToActionResult();
    }
}