using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class TaskModel
{
    [Required]
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime DueDate { get; set; }
    public int Priority { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
    public int Tags { get; set; }
}