using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoadGap.webapi.Data;
using RoadGap.webapi.Dtos;
using RoadGap.webapi.Helpers;
using RoadGap.webapi.Models;
using RoadGap.webapi.Repositories.Implementation;

namespace RoadGapTest.Helpers;

public class EntityCheckerTests
{
    private EntityChecker _entityChecker;
    private DataContext _context;

    [SetUp]
    public void Setup()
    {
        // Set up a new instance of the TaskRepository class before each test
        // We need to pass a configuration object, but for testing purposes we can use an in-memory database
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var context = new DataContext(options);
        _context = context;
        _entityChecker = new EntityChecker(context);
    }

    [TearDown]
    public void TearDown()
    {
        // Dispose of the DbContext after each test
        _context.Tasks.RemoveRange(_context.Tasks);
        _context.Category.RemoveRange(_context.Category);
        _context.Status.RemoveRange(_context.Status);
        _context.SaveChanges();
    }
    
    [Test]
    public void CategoryExists_ValidCategoryId_ReturnsTrue()
    {
        // Arrange
        var category = new Category { CategoryId = 1 };
        var task = new TaskModel { CategoryId = 1 };
        _context.Add(category);
        _context.Add(task);
        _context.SaveChanges();

        // Act
        var result = _entityChecker.CategoryExists(1);

        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void CategoryExists_InvalidCategoryId_ReturnsFalse()
    {
        // Arrange
        const int categoryId = 1;

        // Act
        var result = _entityChecker.CategoryExists(categoryId);

        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void StatusExists_ValidStatusId_ReturnsTrue()
    {
        // Arrange
        var status = new Status { StatusId = 1, Title = "Status 1" };
        _context.Add(status);
        _context.SaveChanges();

        // Act
        var result = _entityChecker.StatusExists(status.StatusId);

        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void StatusExists_InvalidStatusId_ReturnsFalse()
    {
        // Arrange
        const int statusId = 1;

        // Act
        var result = _entityChecker.StatusExists(statusId);

        // Assert
        Assert.That(result, Is.False);
    }
}