using AutoMapper;
using RoadGap.webapi.Data;
using RoadGap.webapi.Helpers;

namespace RoadGap.webapi.Repositories.Implementation;

public abstract class Repository : IRepository
{
    protected readonly DataContext EntityFramework;
    protected readonly IMapper Mapper;
    protected readonly EntityChecker EntityChecker;

    protected Repository(IConfiguration configuration, IMapper mapper)
    {
        EntityFramework = new DataContext(configuration);
        Mapper = mapper;
        EntityChecker = new EntityChecker(EntityFramework);
    }

    protected Repository(DataContext context, IMapper mapper)
    {
        EntityFramework = context;
        Mapper = mapper;
        EntityChecker = new EntityChecker(EntityFramework);
    }

    public DataContext GetDataContext()
    {
        return EntityFramework;
    }

    public void SaveChanges()
    {
        if (EntityFramework.SaveChanges() < 0)
            throw new Exception("Failed to save changes to database");
    }

    public void AddEntity<TEntity>(TEntity entity)
    {
        if (entity == null) return;
        EntityFramework.Add(entity);
    }

    public void RemoveEntity<TEntity>(TEntity entity)
    {
        if (entity == null) return;
        EntityFramework.Remove(entity);
    }

    protected RepositoryResponse<IEnumerable<TEntity>> SearchEntities<TEntity>(
        Func<TEntity, bool> searchPredicate,
        string successMessage,
        string errorMessage)
        where TEntity : class
    {
        try
        {
            var entities = EntityFramework.Set<TEntity>()
                .AsEnumerable().Where(searchPredicate).ToList();

            return RepositoryResponse<IEnumerable<TEntity>>
                .CreateSuccess(entities, successMessage);
        }
        catch (Exception ex)
        {
            return RepositoryResponse<IEnumerable<TEntity>>
                .CreateInternalServerError($"{errorMessage}: {ex.Message}");
        }
    }

    protected RepositoryResponse<TEntity> GetEntityById<TEntity>(int id)
        where TEntity : class
    {
        try
        {
            var entity = EntityFramework.Set<TEntity>().Find(id);

            return entity == null
                ? RepositoryResponse<TEntity>.CreateNotFound($"{typeof(TEntity).Name} with ID {id} not found")
                : RepositoryResponse<TEntity>.CreateSuccess(entity, $"{typeof(TEntity).Name} found successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<TEntity>.CreateInternalServerError(
                $"An error occurred while getting {typeof(TEntity).Name} with ID {id}: {ex.Message}");
        }
    }

    protected RepositoryResponse<TEntity> EditEntity<TEntity, TEntityDto>(int entityId,
        TEntityDto entityDto,
        Func<int, RepositoryResponse<TEntity>> getEntityById,
        Action<TEntityDto, TEntity>? mapEntityDtoToEntity = null)
        where TEntity : class
    {
        try
        {
            var entityResponse = getEntityById(entityId);

            if (!entityResponse.Success)
            {
                return entityResponse;
            }

            var entity = entityResponse.Data;

            if (mapEntityDtoToEntity is not null)
                mapEntityDtoToEntity(entityDto, entity);
            else
                Mapper.Map(entityDto, entity);

            EntityFramework.SaveChanges();

            return RepositoryResponse<TEntity>.CreateSuccess(entity, $"{typeof(TEntity).Name} updated successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<TEntity>.CreateInternalServerError(
                $"An error occurred while editing {typeof(TEntity).Name} with ID {entityId}: {ex.Message}");
        }
    }

    protected RepositoryResponse<TEntity> CreateEntity<TEntity, TEntityDto>
    (TEntityDto entityToAdd,
        Func<TEntityDto, TEntity>? mapEntityDtoToEntity = null)
        where TEntity : class
        where TEntityDto : class
    {
        try
        {
            var entity = mapEntityDtoToEntity is not null
                ? mapEntityDtoToEntity(entityToAdd)
                : Mapper.Map<TEntity>(entityToAdd);

            AddEntity(entity);
            SaveChanges();

            return RepositoryResponse<TEntity>.CreateSuccess(entity,
                $"{typeof(TEntity).Name} created successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<TEntity>.CreateInternalServerError(
                $"An error occurred while creating {typeof(TEntity).Name}: {ex.Message}");
        }
    }

    protected RepositoryResponse<int> DeleteEntity<T>(int entityId)
        where T : class
    {
        try
        {
            var response = GetEntityById<T>(entityId);

            if (!response.Success)
            {
                return response.ConvertWithAnotherData(entityId);
            }

            var entity = response.Data;

            RemoveEntity(entity);
            SaveChanges();

            return RepositoryResponse<int>.CreateSuccess(entityId, $"{typeof(T).Name} deleted successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<int>.CreateInternalServerError(
                $"An error occurred while editing {typeof(T).Name} with ID {entityId}: {ex.Message}");
        }
    }
}