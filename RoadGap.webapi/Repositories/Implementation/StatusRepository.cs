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
        return EditEntity(statusId, statusDto, GetStatusById);
    }

    public RepositoryResponse<Status> CreateStatus(StatusToUpsertDto statusToAdd)
    {
        return CreateEntity<Status, StatusToUpsertDto>(statusToAdd);
    }
    public RepositoryResponse<int> DeleteStatus(int statusId)
    {
        return DeleteEntity<Status>(statusId);
    }}