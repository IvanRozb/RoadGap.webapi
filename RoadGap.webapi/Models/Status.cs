using System.ComponentModel.DataAnnotations;

namespace RoadGap.webapi.Models;

public class Status
{    
    [Required]
    public int StatusId { get; set; }
    public string Title { get; set; } = "";
}