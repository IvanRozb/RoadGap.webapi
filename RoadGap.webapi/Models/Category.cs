using System.ComponentModel.DataAnnotations;

namespace RoadGap.webapi.Models;

public class Category
{
    [Required]
    public int CategoryId { get; set; }
    public string Title { get; set; } = "";
    public string Photo { get; set; } = "";
    public string Description { get; set; } = "";
}