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
    private readonly IMapper _mapper;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        _mapper = new Mapper(new MapperConfiguration(configurationExpression =>
        {
            configurationExpression.CreateMap<CategoryToUpsertDto, Category>();
        }));
    }

    [HttpGet]
    public IActionResult Get([FromQuery] string? searchParam = null)
    {
        var categories = searchParam is null
            ? _categoryRepository.GetCategories()
            : _categoryRepository.GetCategoriesBySearch(searchParam);
        ;
        return Ok(categories);
    }

    [HttpGet("{categoryId:int}")]
    public IActionResult Get(int categoryId)
    {
        var category = _categoryRepository.GetCategoryById(categoryId);
        if (category == null)
        {
            return NotFound("There's no category with this id.");
        }

        return Ok(category);
    }
    
    [HttpPut("{categoryId:int}")]
    public IActionResult Edit(int categoryId, [FromBody] CategoryToUpsertDto categoryDto)
    {
        var categoryDb = _categoryRepository.GetCategoryById(categoryId);

        if (categoryDb == null)
        {
            return NotFound("There's no category with this id.");
        }

        _mapper.Map(categoryDto, categoryDb);

        _categoryRepository.SaveChanges();

        return Ok("Category updated successfully.");
    }

    [HttpPost]
    public IActionResult Create(CategoryToUpsertDto categoryToAdd)
    {
        var category = _mapper.Map<Category>(categoryToAdd);

        _categoryRepository.AddEntity(category);
        _categoryRepository.SaveChanges();

        return Ok("Category created successfully.");
    }

    [HttpDelete("{categoryId:int}")]
    public IActionResult Delete(int categoryId)
    {
        var category = _categoryRepository.GetCategoryById(categoryId);
        
        if (category == null)
        {
            return NotFound("There's no category with this id.");
        }

        _categoryRepository.RemoveEntity(category);
        _categoryRepository.SaveChanges();
        
        return Ok("Category deleted successfully.");
    }
}