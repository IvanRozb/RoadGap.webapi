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
        return _context.Tasks.ToList();
    }
}