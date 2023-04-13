using Microsoft.AspNetCore.Mvc;
using RoadGap.webapi.Data;
using Task = RoadGap.webapi.Models.Task;

namespace RoadGap.webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly DataContext _context;

    public TaskController(IConfiguration configuration)
    {
        _context = new DataContext(configuration);
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
}