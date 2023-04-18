namespace RoadGap.webapi.Dtos;

public class TaskToUpsertDto
{
    public int CategoryId { get; init; }
    public int StatusId { get; init; }
    public string Title { get; init; } = "";
    public string Description { get; init; } = "";
    public DateTime? StartTime { get; set; }
    public DateTime? Deadline { get; set; }
}