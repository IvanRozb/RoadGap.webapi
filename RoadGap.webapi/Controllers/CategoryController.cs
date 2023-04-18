using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoadGap.webapi.Dtos;
using RoadGap.webapi.Models;
using RoadGap.webapi.Repositories;

namespace RoadGap.webapi.Controllers;

[ApiController]
[Route("categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public IActionResult Get([FromQuery] string? searchParam = null)
    {
        var result = 
            _categoryRepository.GetCategories(searchParam ?? "");
        return result.ToActionResult();
    }

    [HttpGet("{categoryId:int}")]
    public IActionResult Get(int categoryId)
    {
        var result = _categoryRepository.GetCategoryById(categoryId);
        return result.ToActionResult();
    }
    
    [HttpPut("{categoryId:int}")]
    public IActionResult Edit(int categoryId, [FromBody] CategoryToUpsertDto categoryDto)
    {
        var result = _categoryRepository.EditCategory(categoryId, categoryDto);
        return result.ToActionResult();
    }

    [HttpPost]
    public IActionResult Create(CategoryToUpsertDto categoryToAdd)
    {
        var result = _categoryRepository.CreateCategory(categoryToAdd);
        return result.ToActionResult();
    }

    [HttpDelete("{categoryId:int}")]
    public IActionResult Delete(int categoryId)
    {
        var result = _categoryRepository.DeleteCategory(categoryId);
        return result.ToActionResult();
    }
}