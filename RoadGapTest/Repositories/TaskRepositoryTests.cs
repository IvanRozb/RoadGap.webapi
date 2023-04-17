using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoadGap.webapi.Data;
using RoadGap.webapi.Dtos;
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
        _taskRepository = new TaskRepository(context, 
            new Mapper(new MapperConfiguration(config => { config.CreateMap<TaskToUpsertDto, TaskModel>(); })));
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
        var tasksRepo = _taskRepository.GetTasks();
        var count = tasksRepo.Data.Count();

        // Assert
        Assert.That(count, Is.EqualTo(0));
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
        Assert.That(_taskRepository.GetTasks().Data.Count(), Is.EqualTo(1));
        Assert.That(_taskRepository.GetTasks().Data.Single().Title, Is.EqualTo("Test task"));
    }
    
    [Test]
    public void RemoveEntity_NullEntity_DoesNotRemoveEntity()
    {
        // Arrange
        var entity = (TaskModel)null;

        // Act
        _taskRepository.RemoveEntity(entity);
        var a = _taskRepository.GetTasks().Data.Count();
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
        Assert.That(_taskRepository.GetTasks().Data.Count(), Is.EqualTo(0));
    }
    
    [Test]
    public void GetTasks_ValidKeyword_ReturnsMatchingTasks()
    {
        // Arrange
        var task1 = new TaskModel { Title = "Test task 1", Description = "Test description 1" };
        var task2 = new TaskModel { Title = "Test task 2", Description = "Test description 2" };
        _taskRepository.AddEntity(task1);
        _taskRepository.AddEntity(task2);
        _taskRepository.SaveChanges();

        // Act
        var results = _taskRepository.GetTasks("test").Data;

        // Assert
        Assert.That(results.Count(), Is.EqualTo(2));
    }
    
    [Test]
    public void GetTasks_InvalidKeyword_ReturnsEmptyList()
    {
        // Arrange
        var task1 = new TaskModel { Title = "Test task 1", Description = "Test description 1" };
        var task2 = new TaskModel { Title = "Test task 2", Description = "Test description 2" };
        _taskRepository.AddEntity(task1);
        _taskRepository.AddEntity(task2);
        _taskRepository.SaveChanges();

        // Act
        var results = _taskRepository.GetTasks("invalid").Data;

        // Assert
        Assert.That(results, Is.Empty);
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
        var result = _taskRepository.GetTaskById(task1.TaskId).Data;

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
        var result = _taskRepository.GetTaskById(-1).Data;

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
        var results = _taskRepository.GetTasks().Data.ToList();

        // Assert
        Assert.That(results, Has.Count.EqualTo(2));
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