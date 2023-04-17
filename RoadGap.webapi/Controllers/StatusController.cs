using Microsoft.AspNetCore.Mvc;
using RoadGap.webapi.Dtos;
using RoadGap.webapi.Repositories;

namespace RoadGap.webapi.Controllers;

[ApiController]
[Route("statuses")]
public class StatusController : ControllerBase
{
    private readonly IStatusRepository _statusRepository;

    public StatusController(IStatusRepository statusRepository)
    {
        _statusRepository = statusRepository;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var result = 
            _statusRepository.GetStatuses();
        return result.ToActionResult();
    }

    [HttpGet("{statusId:int}")]
    public IActionResult Get(int statusId)
    {
        var result = _statusRepository.GetStatusById(statusId);
        return result.ToActionResult();
    }
    
    [HttpPut("{statusId:int}")]
    public IActionResult Edit(int statusId, [FromBody] StatusToUpsertDto statusDto)
    {
        var result = _statusRepository.EditStatus(statusId, statusDto);
        return result.ToActionResult();
    }

    [HttpPost]
    public IActionResult Create(StatusToUpsertDto statusToAdd)
    {
        var result = _statusRepository.CreateStatus(statusToAdd);
        return result.ToActionResult();
    }

    [HttpDelete("{statusId:int}")]
    public IActionResult Delete(int statusId)
    {
        var result = _statusRepository.DeleteStatus(statusId);
        return result.ToActionResult();
    }
}