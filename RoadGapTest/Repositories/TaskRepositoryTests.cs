using Microsoft.EntityFrameworkCore;
using RoadGap.webapi.Data;
using RoadGap.webapi.Models;
using RoadGap.webapi.Repositories.Implementation;

namespace RoadGapTest.Repositories;

[TestFixture]
public class TaskRepositoryTests
{
    private TaskRepository _taskRepository;

    [SetUp]
    public void Setup()
    {
        // Set up a new instance of the TaskRepository class before each test
        // We need to pass a configuration object, but for testing purposes we can use an in-memory database
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var context = new DataContext(options);
        _taskRepository = new TaskRepository(context);
    }

    [TearDown]
    public void TearDown()
    {
        // Dispose of the DbContext after each test
        _taskRepository.Dispose();
    }
    
    [Test]
    public void AddEntity_NullEntity_DoesNotAddEntity()
    {
        // Arrange
        var entity = (TaskModel)null;

        // Act
        _taskRepository.AddEntity(entity);

        // Assert
        Assert.That(_taskRepository.GetTasks().Count(), Is.EqualTo(0));
    }
    
    [Test]
    public void AddEntity_ValidEntity_AddsEntity()
    {
        // Arrange
        var entity = new TaskModel { Title = "Test task" };

        // Act
        _taskRepository.AddEntity(entity);
        _taskRepository.SaveChanges();

        // Assert
        Assert.That(_taskRepository.GetTasks().Count(), Is.EqualTo(1));
        Assert.That(_taskRepository.GetTasks().Single().Title, Is.EqualTo("Test task"));
    }
    
    [Test]
    public void RemoveEntity_NullEntity_DoesNotRemoveEntity()
    {
        // Arrange
        var entity = (TaskModel)null;

        // Act
        _taskRepository.RemoveEntity(entity);
        var a = _taskRepository.GetTasks().Count();
        // Assert
        Assert.That(a, Is.EqualTo(0));
    }

    [Test]
    public void RemoveEntity_ValidEntity_RemovesEntity()
    {
        // Arrange
        var entity = new TaskModel { Title = "Test task" };
        _taskRepository.AddEntity(entity);
        _taskRepository.SaveChanges();

        // Act
        _taskRepository.RemoveEntity(entity);
        _taskRepository.SaveChanges();

        // Assert
        Assert.That(_taskRepository.GetTasks().Count(), Is.EqualTo(0));
    }
    
    [Test]
    public void GetTasksBySearch_ValidKeyword_ReturnsMatchingTasks()
    {
        // Arrange
        var task1 = new TaskModel { Title = "Test task 1", Description = "Test description 1" };
        var task2 = new TaskModel { Title = "Test task 2", Description = "Test description 2" };
        _taskRepository.AddEntity(task1);
        _taskRepository.AddEntity(task2);
        _taskRepository.SaveChanges();

        // Act
        var results = _taskRepository.GetTasksBySearch("test");

        // Assert
        Assert.That(results.Count(), Is.EqualTo(2));
    }
    
    [Test]
    public void GetTasksBySearch_InvalidKeyword_ReturnsEmptyList()
    {
        // Arrange
        var task1 = new TaskModel { Title = "Test task 1", Description = "Test description 1" };
        var task2 = new TaskModel { Title = "Test task 2", Description = "Test description 2" };
        _taskRepository.AddEntity(task1);
        _taskRepository.AddEntity(task2);
        _taskRepository.SaveChanges();

        // Act
        var results = _taskRepository.GetTasksBySearch("invalid");

        // Assert
        Assert.That(results, Is.Empty);
    }
    
    [Test]
    public void CategoryExists_ValidCategoryId_ReturnsTrue()
    {
        // Arrange
        var category = new Category { CategoryId = 1 };
        var task = new TaskModel { CategoryId = 1 };
        _taskRepository.AddEntity(category);
        _taskRepository.AddEntity(task);
        _taskRepository.SaveChanges();

        // Act
        var result = _taskRepository.CategoryExists(1);

        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void CategoryExists_InvalidCategoryId_ReturnsFalse()
    {
        // Arrange
        const int categoryId = 1;

        // Act
        var result = _taskRepository.CategoryExists(categoryId);

        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void StatusExists_ValidStatusId_ReturnsTrue()
    {
        // Arrange
        var status = new Status { StatusId = 1, Title = "Status 1" };
        _taskRepository.AddEntity(status);
        _taskRepository.SaveChanges();

        // Act
        var result = _taskRepository.StatusExists(status.StatusId);

        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void StatusExists_InvalidStatusId_ReturnsFalse()
    {
        // Arrange
        const int statusId = 1;

        // Act
        var result = _taskRepository.StatusExists(statusId);

        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void GetTaskById_ValidId_ReturnsMatchingTask()
    {
        // Arrange
        var task1 = new TaskModel { Title = "Test task 1" };
        var task2 = new TaskModel { Title = "Test task 2" };
        _taskRepository.AddEntity(task1);
        _taskRepository.AddEntity(task2);
        _taskRepository.SaveChanges();

        // Act
        var result = _taskRepository.GetTaskById(task1.TaskId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.TaskId, Is.EqualTo(task1.TaskId));
    }
    
    [Test]
    public void GetTaskById_InvalidId_ReturnsNull()
    {
        // Arrange
        var task1 = new TaskModel { Title = "Test task 1" };
        var task2 = new TaskModel { Title = "Test task 2" };
        _taskRepository.AddEntity(task1);
        _taskRepository.AddEntity(task2);
        _taskRepository.SaveChanges();

        // Act
        var result = _taskRepository.GetTaskById(-1);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetTasks_ReturnsAllTasks()
    {
        // Arrange
        var task1 = new TaskModel { Title = "Test task 1" };
        var task2 = new TaskModel { Title = "Test task 2" };
        _taskRepository.AddEntity(task1);
        _taskRepository.AddEntity(task2);
        _taskRepository.SaveChanges();

        // Act
        var results = _taskRepository.GetTasks();

        // Assert
        Assert.That(results.Count(), Is.EqualTo(2));
        Assert.That(results.Any(t => t.Title == "Test task 1"), Is.True);
        Assert.That(results.Any(t => t.Title == "Test task 2"), Is.True);
    }
    
    [Test]
    public void SaveChanges_SavesChangesToDatabase()
    {
        // Arrange
        var task = new TaskModel { Title = "Test task" };
        _taskRepository.AddEntity(task);

        // Act
        _taskRepository.SaveChanges();

        // Assert
        var context = _taskRepository.GetDataContext();
        Assert.That(context.ChangeTracker.HasChanges(), Is.False);
        Assert.That(context.Tasks.Count(), Is.EqualTo(1));
    }
}