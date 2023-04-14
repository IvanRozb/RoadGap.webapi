using System.ComponentModel.DataAnnotations;

namespace RoadGap.webapi.Models;

public class TaskModel
{
    [Required]
    public int TaskId { get; set; }
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public int StatusId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime? StartTime { get; set; }
    public DateTime? Deadline { get; set; }
    public DateTime? TaskUpdated { get; set; }
}