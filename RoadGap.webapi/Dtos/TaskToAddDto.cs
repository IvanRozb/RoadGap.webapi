namespace RoadGap.webapi.Dtos;

public class TaskToAddDto
{
    public int CategoryId { get; set; }
    public int StatusId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime? StartTime { get; set; }
    public DateTime? Deadline { get; set; }
    public DateTime? TaskUpdated { get; set; }
}