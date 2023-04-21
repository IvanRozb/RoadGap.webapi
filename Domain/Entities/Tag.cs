using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Tag
{
    [Required]
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}