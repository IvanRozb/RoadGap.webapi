using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Project
{
    [Required]
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid UserId { get; set; }
    public ICollection<TaskModel> Tasks { get; set; }
}