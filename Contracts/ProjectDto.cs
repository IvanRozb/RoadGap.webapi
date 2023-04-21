using Domain.Entities;

namespace Contracts;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid UserId { get; set; }
    public IEnumerable<TaskModel> Tasks { get; set; }
}