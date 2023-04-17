using RoadGap.webapi.Dtos;
using RoadGap.webapi.Helpers;
using RoadGap.webapi.Models;

namespace RoadGap.webapi.Repositories;

public interface IStatusRepository
{
    public RepositoryResponse<IEnumerable<Status>> GetStatuses();
    public RepositoryResponse<Status> GetStatusById(int statusId);
    public RepositoryResponse<Status> EditStatus(int statusId, StatusToUpsertDto statusDto);
    public RepositoryResponse<Status> CreateStatus(StatusToUpsertDto statusToAdd);
    public RepositoryResponse<int> DeleteStatus(int statusId);
}