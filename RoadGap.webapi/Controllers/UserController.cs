using Microsoft.AspNetCore.Mvc;
using RoadGap.webapi.Dtos;
using RoadGap.webapi.Repositories;

namespace RoadGap.webapi.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [HttpGet]
    public IActionResult Get([FromQuery] string? searchParam = null)
    {
        var result = 
            _userRepository.GetUsers(searchParam ?? "");
        return result.ToActionResult();
    }

    [HttpGet("{userId:int}")]
    public IActionResult Get(int userId)
    {
        var result = _userRepository.GetUserById(userId);
        return result.ToActionResult();
    }
    
    [HttpPut("{userId:int}")]
    public IActionResult Edit(int userId, [FromBody] UserToUpsertDto userDto)
    {
        var result = _userRepository.EditUser(userId, userDto);
        return result.ToActionResult();
    }

    [HttpPost]
    public IActionResult Create(UserToUpsertDto userToAdd)
    {
        var result = _userRepository.CreateUser(userToAdd);
        return result.ToActionResult();
    }

    [HttpDelete("{userId:int}")]
    public IActionResult Delete(int userId)
    {
        var result = _userRepository.DeleteUser(userId);
        return result.ToActionResult();
    }
}