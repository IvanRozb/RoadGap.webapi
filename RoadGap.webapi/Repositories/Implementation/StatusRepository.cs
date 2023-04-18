using AutoMapper;
using RoadGap.webapi.Data;
using RoadGap.webapi.Dtos;
using RoadGap.webapi.Helpers;
using RoadGap.webapi.Models;

namespace RoadGap.webapi.Repositories.Implementation;

public class StatusRepository : Repository, IStatusRepository
{
    public StatusRepository(IConfiguration configuration, IMapper mapper)
        : base(configuration, mapper)
    {
    }
    
    public StatusRepository(DataContext context, IMapper mapper) 
        : base(context, mapper)
    {
    }
    
    public void Dispose()
    {
        EntityFramework.Status.RemoveRange(EntityFramework.Status);
        SaveChanges();
    }
    
    public RepositoryResponse<IEnumerable<Status>> GetStatuses()
    {
        bool SearchPredicate(Status status) => true;

        return SearchEntities((Func<Status, bool>)SearchPredicate, "Statuses found successfully.", "An error occurred while getting statuses.");
    }
    public RepositoryResponse<Status> GetStatusById(int statusId)
    {
        try
        {
            var status = EntityFramework.Status
                .FirstOrDefault(status => status.StatusId == statusId);

            if (status == null)
            {
                return RepositoryResponse<Status>.CreateNotFound($"Status with ID {statusId} not found");
            }

            return RepositoryResponse<Status>
                .CreateSuccess(status, "Status found successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<Status>.CreateInternalServerError($"An error occurred while getting status with ID {statusId}: {ex.Message}");
        }
    }
    
    public RepositoryResponse<Status> EditStatus(int statusId, StatusToUpsertDto statusDto)
    {
        try
        {
            var statusResponse = GetStatusById(statusId);

            if (!statusResponse.Success)
            {
                return statusResponse;
            }

            var status = statusResponse.Data;
            
            Mapper.Map(statusDto, status);

            EntityFramework.SaveChanges();

            return RepositoryResponse<Status>.CreateSuccess(status, "Status updated successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<Status>.CreateInternalServerError($"An error occurred while editing status with ID {statusId}: {ex.Message}");
        }
    }
    public RepositoryResponse<Status> CreateStatus(StatusToUpsertDto statusToAdd)
    {
        try
        {
            var status = Mapper.Map<Status>(statusToAdd);
            
            AddEntity(status);
            SaveChanges();

            return RepositoryResponse<Status>.CreateSuccess(status, "Status created successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<Status>.CreateInternalServerError($"An error occurred while creating status: {ex.Message}");
        }
    }
    public RepositoryResponse<int> DeleteStatus(int statusId)
    {
        try
        {
            var response = GetStatusById(statusId);

            if (!response.Success)
            {
                return RepositoryResponse<int>.CreateBadRequest($"Status with ID {statusId} not found");
            }

            var status = response.Data;

            RemoveEntity(status);
            SaveChanges();

            return RepositoryResponse<int>.CreateSuccess(statusId, "Status deleted successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<int>.CreateInternalServerError($"An error occurred while editing status with ID {statusId}: {ex.Message}");
        }
    }
}