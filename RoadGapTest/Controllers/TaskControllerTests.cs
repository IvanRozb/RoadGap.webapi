using Microsoft.AspNetCore.Mvc;
using Moq;
using RoadGap.webapi.Controllers;
using RoadGap.webapi.Dtos;
using RoadGap.webapi.Models;
using RoadGap.webapi.Repositories;

namespace RoadGapTest.Controllers;

[TestFixture]
public class TaskControllerTests
{
    private TaskController _taskController = null!;
    private Mock<ITaskRepository> _mock = null!;

    [SetUp]
    public void Setup()
    {
        _mock = new Mock<ITaskRepository>();
        _taskController = new TaskController(_mock.Object);
    }

    [Test]
    public void Get_ReturnsOkResult()
    {
        // Arrange
        _mock.Setup(x => x.GetTasks())
            .Returns(new[] { new TaskModel(), new TaskModel() });
        
        //Act
        var result = _taskController.Get() as OkObjectResult;
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.Not.Null);
        });
    }

    [Test]
    public void Get_WithNoTasks_ReturnsEmptyList()
    {
        // Arrange
        _mock.Setup(x => x.GetTasks())
            .Returns(new List<TaskModel>());
        
        //Act
        var result = _taskController.Get() as OkObjectResult;
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Is.InstanceOf<List<TaskModel>>());

        var task = result.Value as List<TaskModel>;
        Assert.That(task, Is.Empty);
    }

    [Test]
    public void Get_WithValidTaskId_ReturnsOkResult()
    {
        // Arrange
        _mock.Setup(x => x.GetTaskById(It.IsAny<int>()))
            .Returns(new TaskModel(){TaskId = 2});
        
        //Act
        var result = _taskController.Get(1) as OkObjectResult;
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.Not.Null);
        });
    }

    [Test]
    public void Get_WithInvalidTaskId_ReturnsNotFoundResult()
    {
        // Arrange
        _mock.Setup(x => x.GetTaskById(It.IsAny<int>()))
            .Returns<TaskModel>(null);
        
        //Act
        var result = _taskController.Get(1) as NotFoundObjectResult;
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(404));
        Assert.That(result.Value, Is.EqualTo("There's no task with this id."));
    }

    [Test]
    public void Get_WithSearchParam_ReturnsOkResult()
    {
        // Arrange
        const string searchParam = "searchParam";
        _mock.Setup(x => x.GetTasksBySearch(searchParam))
            .Returns(new[] { new TaskModel { Title = "1 or searchParam or 2" } });
        
        //Act
        var result = _taskController.Get(searchParam) as OkObjectResult;
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.Not.Null);
        });
        if (result!.Value is not List<TaskModel> tasks) return;
        foreach (var task in tasks)
        {
            Assert.That(task.Title, Does.Contain(searchParam));
        }
    }

    [Test]
    public void Get_WithNoSearchMatches_ReturnsEmptyList()
    {
        // Arrange
        const string searchParam = "searchParam";
        _mock.Setup(x => x.GetTasksBySearch(searchParam))
            .Returns(Array.Empty<TaskModel>());
    
        //Act
        var result = _taskController.Get(searchParam) as OkObjectResult;
    
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.Empty);
        });
    }

    [Test]
    public void Edit_WithValidTaskIdAndValidTaskToUpsertDto_ReturnsOkResult()
    {
        // Arrange
        var taskToUpsertDto = new TaskToUpsertDto
        {
            Title = "Task Title",
            Description = "Task Description",
            CategoryId = 1,
            StatusId = 1
        };
        _mock.Setup(x => x.CategoryExists(It.IsAny<int>())).Returns(true);
        _mock.Setup(x => x.StatusExists(It.IsAny<int>())).Returns(true);
        _mock.Setup(x => x.GetTaskById(It.IsAny<int>())).Returns(new TaskModel());

        // Act
        var result = _taskController.Edit(1, taskToUpsertDto) as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo("Task updated successfully."));
        });
    }

    [Test]
    [TestCase(false,true, "Invalid category id.")]
    [TestCase(true,false, "Invalid status id.")]
    public void Edit_WithValidTaskIdAndInvalidTaskToUpsertDto_ReturnsBadRequestResult
        (bool categoryExists, bool statusExists, string message)
    {
        // Arrange
        var taskToUpsertDto = new TaskToUpsertDto
        {
            Title = "Task Title",
            Description = "Task Description",
            CategoryId = 1,
            StatusId = 1
        };
        _mock.Setup(x => x.CategoryExists(It.IsAny<int>())).Returns(categoryExists);
        _mock.Setup(x => x.StatusExists(It.IsAny<int>())).Returns(statusExists);
        _mock.Setup(x => x.GetTaskById(It.IsAny<int>())).Returns(new TaskModel());

        // Act
        var result = _taskController.Edit(1, taskToUpsertDto) as BadRequestObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(400));
        Assert.That(result.Value, Is.EqualTo(message));
    }
    
    [Test]
    public void Edit_WithInvalidTaskId_ReturnsNotFoundResult()
    {
        // Arrange
        var taskToUpsertDto = new TaskToUpsertDto
        {
            Title = "Task Title",
            Description = "Task Description",
            CategoryId = 1,
            StatusId = 1
        };
        _mock.Setup(x => x.CategoryExists(It.IsAny<int>())).Returns(true);
        _mock.Setup(x => x.StatusExists(It.IsAny<int>())).Returns(true);
        _mock.Setup(x => x.GetTaskById(It.IsAny<int>())).Returns<TaskModel>(null);

        // Act
        var result = _taskController.Edit(1, taskToUpsertDto) as NotFoundObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(404));
        Assert.That(result.Value, Is.EqualTo("There's no task with this id."));
    }
    
    [Test]
    public void Delete_WithValidTaskId_ReturnsOkResult()
    {
        // Arrange
        const int taskId = 1;
        _mock.Setup(x => x.GetTaskById(taskId)).Returns(new TaskModel { TaskId = taskId });

        // Act
        var result = _taskController.Delete(taskId) as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.EqualTo("Task deleted successfully."));
    }
    
    [Test]
    public void Delete_WithInvalidTaskId_ReturnsNotFoundResult()
    {
        // Arrange
        const int taskId = 1;
        _mock.Setup(x => x.GetTaskById(taskId)).Returns<TaskModel>(null);

        // Act
        var result = _taskController.Delete(taskId) as NotFoundObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(404));
        Assert.That(result.Value, Is.EqualTo("There's no task with this id."));
    }
    
    [Test]
    public void Create_ReturnsOkResult()
    {
        // Arrange
        var task = new TaskToUpsertDto();
        _mock.Setup(x => x.CategoryExists(It.IsAny<int>())).Returns(true);
        _mock.Setup(x => x.StatusExists(It.IsAny<int>())).Returns(true);

        // Act
        var result = _taskController.Create(task);
        var createdResult = result as OkObjectResult;

        // Assert
        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult.StatusCode, Is.EqualTo(200));
        Assert.That(createdResult.Value, Is.Not.Null);
        Assert.That(createdResult.Value, Is.EqualTo("Task created successfully."));
    }
    
    [Test]
    [TestCase(false,true, "Invalid category id.")]
    [TestCase(true,false, "Invalid status id.")]
    public void Create_WithInvalidTaskToUpsertDto_ReturnsBadRequestResult
        (bool categoryExists, bool statusExists, string message)
    {
        // Arrange
        var task = new TaskToUpsertDto();
        _mock.Setup(x => x.CategoryExists(It.IsAny<int>())).Returns(categoryExists);
        _mock.Setup(x => x.StatusExists(It.IsAny<int>())).Returns(statusExists);

        // Act
        var result = _taskController.Create(task);
        var createdResult = result as BadRequestObjectResult;

        // Assert
        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult.StatusCode, Is.EqualTo(400));
        Assert.That(createdResult.Value, Is.Not.Null);
        Assert.That(createdResult.Value, Is.EqualTo(message));
    }

}