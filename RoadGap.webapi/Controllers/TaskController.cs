using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoadGap.webapi.Data;
using RoadGap.webapi.Dtos;
using Task = RoadGap.webapi.Models.Task;

namespace RoadGap.webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public TaskController(IConfiguration configuration)
    {
        _context = new DataContext(configuration);
        _mapper = new Mapper(new MapperConfiguration(configurationExpression =>
        {
            configurationExpression.CreateMap<TaskToUpsertDto, Task>();
        }));
    }
    
    [HttpGet("GetTasks")]
    public IEnumerable<Task> GetTasks()
    {
        return _context.Tasks;
    }

    [HttpGet("GetTaskById/{taskId:int}")]
    public IActionResult GetTaskById(int taskId)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.TaskId == taskId);

        if (task == null)
        {
            return NotFound($"No task found with ID = {taskId}");
        }

        return Ok(task);
    }

    [HttpGet("GetTasksBySearch/{searchParam}")]
    public IActionResult GetTasksBySearch(string searchParam)
    {
        var keywords = searchParam.Split(' ');
        var tasks = _context.Tasks;
        var searchedTasks = new List<Task>();
        
        foreach (var keyword in keywords)
        {
            searchedTasks.AddRange(tasks.Where(task =>
                task.Title.Contains(keyword) ||
                task.Description.Contains(keyword))
                .ToList());
        }

        return Ok(searchedTasks);
    }
    
    [HttpPut("EditTask/{taskId}")]
    public IActionResult EditTask(int taskId, [FromBody] TaskToUpsertDto taskDto)
    {
        if (!_context.Category.Any(c => c.CategoryId == taskDto.CategoryId))
        {
            return BadRequest("Invalid category id.");
        }

        if (!_context.Status.Any(s => s.StatusId == taskDto.StatusId))
        {
            return BadRequest("Invalid status id.");
        }

        var taskDb = _context.Tasks
            .FirstOrDefault(t => t.TaskId == taskId);

        if (taskDb == null)
        {
            return NotFound("Task not found.");
        }

        _mapper.Map(taskDto, taskDb);

        _context.SaveChanges();

        return Ok("Task updated successfully.");
    }

    [HttpPost("CreateTask")]
    public IActionResult CreateTask(TaskToUpsertDto taskToAdd)
    {
        if (!_context.Category.Any(c => c.CategoryId == taskToAdd.CategoryId))
        {
            return BadRequest("Invalid category id.");
        }

        if (!_context.Status.Any(s => s.StatusId == taskToAdd.StatusId))
        {
            return BadRequest("Invalid status id.");
        }
        
        var task = _mapper.Map<Task>(taskToAdd);
        
        _context.Tasks.Add(task);
        _context.SaveChanges();
        
        return Ok("Task created successfully.");
    }

    [HttpDelete("DeleteTask/{taskId:int}")]
    public IActionResult DeleteTask(int taskId)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.TaskId == taskId);
        if (task == null)
        {
            return NotFound("Task not found.");
        }

        _context.Tasks.Remove(task);
        _context.SaveChanges();
        
        return Ok("Task deleted successfully.");
    }
}