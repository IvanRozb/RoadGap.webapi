using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoadGap.webapi.Data;
using RoadGap.webapi.Dtos;
using RoadGap.webapi.Models;
using RoadGap.webapi.Repositories.Implementation;

namespace RoadGapTest.Repositories;

[TestFixture]
public class CategoryRepositoryTests
{
    private CategoryRepository _categoryRepository;
    
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var context = new DataContext(options);
        _categoryRepository = new CategoryRepository(context,
            new Mapper(new MapperConfiguration(config =>
            {
                config.CreateMap<CategoryToUpsertDto, Category>();
            })));
    }
    
    [TearDown]
    public void TearDown()
    {
        _categoryRepository.Dispose();
    }
    
    [Test]
    public void GetCategories_NoSearchParam_ReturnsAllCategories()
    {
        // Arrange
        var category1 = new Category { Title = "Test1", Description = "Test1" };
        var category2 = new Category { Title = "Test2", Description = "Test2" };
        _categoryRepository.CreateCategory(new CategoryToUpsertDto { Title = category1.Title, Description = category1.Description });
        _categoryRepository.CreateCategory(new CategoryToUpsertDto { Title = category2.Title, Description = category2.Description });

        // Act
        var result = _categoryRepository.GetCategories();

        // Assert
        Assert.That(result.Success);
        Assert.That(result.Data.Count(), Is.EqualTo(2));
        Assert.That(result.Data.ElementAt(1).Title, Is.EqualTo(category1.Title));
        Assert.That(result.Data.ElementAt(0).Title, Is.EqualTo(category2.Title));
    }
    
    [Test]
    public void GetCategories_WithSearchParam_ReturnsMatchingCategories()
    {
        // Arrange
        var category1 = new Category { Title = "Test1", Description = "Test1" };
        var category2 = new Category { Title = "Test2", Description = "Test2" };
        _categoryRepository.CreateCategory(new CategoryToUpsertDto { Title = category1.Title, Description = category1.Description });
        _categoryRepository.CreateCategory(new CategoryToUpsertDto { Title = category2.Title, Description = category2.Description });

        // Act
        var result = _categoryRepository.GetCategories("test1");

        // Assert
        Assert.That(result.Success);
        Assert.That(result.Data.Count(), Is.EqualTo(1));
        Assert.That(result.Data.ElementAt(0).Title, Is.EqualTo(category1.Title));
    }
    
    [Test]
    public void GetCategoryById_NonExistingCategoryId_ReturnsNotFound()
    {
        // Arrange
        const int categoryId = 99; // Assuming there is no category with ID 99 in the in-memory database

        // Act
        var result = _categoryRepository.GetCategoryById(categoryId);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Is.EqualTo($"Category with ID {categoryId} not found"));
    }

    [Test]
    public void GetCategoryById_ExistingCategoryId_ReturnsCategory()
    {
        // Arrange
        var category1 = new Category { CategoryId = 1, Title = "Test1", Description = "Test1" };
        var category2 = new Category { CategoryId = 2, Title = "Test2", Description = "Test2" };
        var response = _categoryRepository.CreateCategory(new CategoryToUpsertDto
            { Title = category1.Title, Description = category1.Description });
        _categoryRepository.CreateCategory(new CategoryToUpsertDto
            { Title = category2.Title, Description = category2.Description });
       
        // Act
        var result = _categoryRepository.GetCategoryById(response.Data.CategoryId);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.EqualTo(response.Data));
        Assert.That(result.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public void EditCategory_ExistingCategoryIdAndValidDto_UpdatesCategory()
    {
        // Arrange
        var category1 = new Category { CategoryId = 1, Title = "Test1", Description = "Test1" };
        var category2 = new Category { CategoryId = 2, Title = "Test2", Description = "Test2" };
        var categoryDto = new CategoryToUpsertDto
        {
            Title = "Madagascar",
            Description = category1.Description
        };
        var response = _categoryRepository.CreateCategory(new CategoryToUpsertDto
            { Title = category1.Title, Description = category1.Description });
        _categoryRepository.CreateCategory(new CategoryToUpsertDto
            { Title = category2.Title, Description = category2.Description });

        // Act
        var result = _categoryRepository.EditCategory(response.Data.CategoryId, categoryDto);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.EqualTo(response.Data));
        Assert.That(result.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public void EditCategory_NonExistingCategoryId_ReturnsNotFound()
    {
        // Arrange
        const int categoryId = 99; // Assuming there is no category with ID 99 in the in-memory database
        var categoryDto = new CategoryToUpsertDto();

        // Act
        var result = _categoryRepository.EditCategory(categoryId, categoryDto);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Is.EqualTo($"Category with ID {categoryId} not found"));
    }

    [Test]
    public void CreateCategory_CreatesCategory()
    {
        // Arrange
        var categoryDto = new CategoryToUpsertDto { Title = "Madagascar", 
            Description = "An island" };
        // Act
        var result = _categoryRepository.CreateCategory(categoryDto);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Data.Title, Is.EqualTo(categoryDto.Title));
        Assert.That(result.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public void DeleteCategory_ExistingCategoryId_DeletesCategory()
    {
        var categoryDto = new CategoryToUpsertDto { Title = "Madagascar", 
            Description = "An island" };
        var response = _categoryRepository.CreateCategory(categoryDto);

        var result = _categoryRepository
            .DeleteCategory(response.Data.CategoryId);
        
        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.EqualTo(response.Data.CategoryId));
        Assert.That(result.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public void DeleteCategory_NonExistingCategoryId_ReturnsBadRequest()
    {
        const int nonExistingId = 99;
        var result = _categoryRepository
            .DeleteCategory(nonExistingId);
        
        Assert.That(result.Success, Is.False);
        Assert.That(result.StatusCode, Is.EqualTo(400));
    }
}