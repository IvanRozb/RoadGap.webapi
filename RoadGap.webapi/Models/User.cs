using System.ComponentModel.DataAnnotations;

namespace RoadGap.webapi.Models;

public class User
{
    [Required]
    public int UserId { get; set; }
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Photo { get; set; } = "";
}