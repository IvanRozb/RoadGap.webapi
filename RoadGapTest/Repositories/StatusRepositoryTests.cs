using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoadGap.webapi.Data;
using RoadGap.webapi.Dtos;
using RoadGap.webapi.Models;
using RoadGap.webapi.Repositories.Implementation;

namespace RoadGapTest.Repositories;

public class StatusRepositoryTests
{
    private StatusRepository _statusRepository;
    
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var context = new DataContext(options);
        _statusRepository = new StatusRepository(context,
            new Mapper(new MapperConfiguration(config =>
            {
                config.CreateMap<StatusToUpsertDto, Status>();
            })));
    }
    
    [TearDown]
    public void TearDown()
    {
        _statusRepository.Dispose();
    }
    
    [Test]
    public void GetStatuses_ReturnsAllStatuses()
    {
        var status1 = new Status { StatusId = 1, Title = "Test1" };
        var status2 = new Status { StatusId = 2, Title = "Test2" };
        _statusRepository.CreateStatus(new StatusToUpsertDto
        {
            Title = status1.Title
        });
        _statusRepository.CreateStatus(new StatusToUpsertDto
        {
            Title = status2.Title
        });

        var result = _statusRepository.GetStatuses();
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Statuses found successfully."));
            Assert.That(result.Data.Count(), Is.EqualTo(2));
            Assert.That(result.Data.ElementAt(1).Title, Is.EqualTo(status1.Title));
            Assert.That(result.Data.ElementAt(0).Title, Is.EqualTo(status2.Title));
        });
    }

    [Test]
    public void GetStatusById_ExistingStatusId_ReturnsStatus()
    {
        var status1 = new Status { StatusId = 1, Title = "Test1"};
        var status2 = new Status { StatusId = 2, Title = "Test2"};
        var res = _statusRepository.CreateStatus(new StatusToUpsertDto
        {
            Title = status1.Title
        });
        _statusRepository.CreateStatus(new StatusToUpsertDto
        {
            Title = status2.Title
        });
        
        var result = _statusRepository.GetStatusById(res.Data.StatusId);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Status found successfully."));
            Assert.That(result.Data.Title, Is.EqualTo(status1.Title));
            Assert.That(result.StatusCode, Is.EqualTo(200));
        });
    }

    [Test]
    public void GetStatusById_NonExistingStatusId_ReturnsNotFound()
    {
        const int nonExistingStatusId = 999;

        var result = _statusRepository.GetStatusById(nonExistingStatusId);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo($"Status with ID {nonExistingStatusId} not found"));
            Assert.That(result.StatusCode, Is.EqualTo(404));
        });
    }

    [Test]
    public void EditStatus_ExistingStatusId_ReturnsUpdatedStatus()
    {
        var res = _statusRepository.CreateStatus(new StatusToUpsertDto { Title = "status1" });
        _statusRepository.SaveChanges();
        
        var response = _statusRepository
            .EditStatus(res.Data.StatusId, new StatusToUpsertDto { Title = "status2" });
        Assert.Multiple(() =>
        {
            Assert.That(response.Success);
            Assert.That(response.Message, Is.EqualTo("Status updated successfully."));
        });
        var updatedStatus = response.Data;
        Assert.That(updatedStatus.Title, Is.EqualTo("status2"));
    }

    [Test]
    public void EditStatus_NonExistingStatusId_ReturnsNotFound()
    {
        const int nonExistingId = 1;
        var statusDto = new StatusToUpsertDto { Title = "New Status Title" };

        var result = _statusRepository
            .EditStatus(nonExistingId, statusDto);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo($"Status with ID {nonExistingId} not found"));
        });
    }

    [Test]
    public void CreateStatus_ReturnsCreatedStatus()
    {
        var statusDto = new StatusToUpsertDto { Title = "New Status Title" };

        var result = _statusRepository.CreateStatus(statusDto);
        Assert.Multiple(() =>
        {
            Assert.That(result.Success);
            Assert.That(result.Data.Title, Is.EqualTo(statusDto.Title));
        });
    }

    [Test]
    public void DeleteStatus_ExistingStatusId_ReturnsDeletedStatusId()
    {
        var res = _statusRepository.CreateStatus(new StatusToUpsertDto());

        var result = _statusRepository.DeleteStatus(res.Data.StatusId);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Success);
            Assert.That(result.Data, Is.EqualTo(res.Data.StatusId));
        });
    }
    
    [Test]
    public void DeleteStatus_NonExistingStatusId_ReturnsNotFound()
    {
        const int nonExistingStatusId = 99;
        
        var result = _statusRepository.DeleteStatus(nonExistingStatusId);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.StatusCode, Is.EqualTo(404));
        });
    }
}