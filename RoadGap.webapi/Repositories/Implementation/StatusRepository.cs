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
        return GetEntityById<Status>(statusId);
    }

    public RepositoryResponse<Status> EditStatus(int statusId, StatusToUpsertDto statusDto)
    {
        return EditEntity(statusId, statusDto, GetStatusById, (dto, entity) => {
            Mapper.Map(dto, entity);
        });
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