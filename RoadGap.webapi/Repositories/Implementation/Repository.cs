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

    public void AddEntity<T>(T entity)
    {
        if (entity == null) return;
        EntityFramework.Add(entity);
    }
    
    public void RemoveEntity<T>(T entity)
    {
        if (entity == null) return;
        EntityFramework.Remove(entity);
    }

    protected RepositoryResponse<IEnumerable<T>> SearchEntities<T>(
        Func<T, bool> searchPredicate,
        string successMessage,
        string errorMessage)
        where T : class
    {
        try
        {
            var entities = EntityFramework.Set<T>()
                .AsEnumerable().Where(searchPredicate).ToList();

            return RepositoryResponse<IEnumerable<T>>
                .CreateSuccess(entities, successMessage);
        }
        catch (Exception ex)
        {
            return RepositoryResponse<IEnumerable<T>>
                .CreateInternalServerError($"{errorMessage}: {ex.Message}");
        }
    }

    protected RepositoryResponse<T> GetEntityById<T>(int id) where T : class
    {
        try
        {
            var entity = EntityFramework.Set<T>().Find(id);

            return entity == null
                ? RepositoryResponse<T>.CreateNotFound($"{typeof(T).Name} with ID {id} not found")
                : RepositoryResponse<T>.CreateSuccess(entity, $"{typeof(T).Name} found successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<T>.CreateInternalServerError(
                $"An error occurred while getting {typeof(T).Name} with ID {id}: {ex.Message}");
        }
    }

    protected RepositoryResponse<TEntity> EditEntity<TEntity, TEntityDto>(int entityId, TEntityDto entityDto,
        Func<int, RepositoryResponse<TEntity>> getEntityById, Action<TEntityDto, TEntity>? mapEntityDtoToEntity = null)
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

    protected RepositoryResponse<T> CreateEntity<T, TD>(TD entityToAdd, Func<TD, bool>[]? validationChecks = null,
        string[]? errorMessages = null)
        where T : class
        where TD : class
    {
        try
        {
            if (validationChecks is not null)
            {
                errorMessages ??= Enumerable
                    .Repeat("Validation failed", validationChecks.Length)
                    .ToArray();

                for (var i = 0; i < validationChecks.Length; i++)
                {
                    if (validationChecks[i](entityToAdd))
                    {
                        return RepositoryResponse<T>.CreateConflict(errorMessages[i]);
                    }
                }
            }

            var entity = Mapper.Map<T>(entityToAdd);

            AddEntity(entity);
            SaveChanges();

            return RepositoryResponse<T>.CreateSuccess(entity, $"{typeof(T).Name} created successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<T>.CreateInternalServerError(
                $"An error occurred while creating {typeof(T).Name}: {ex.Message}");
        }
    }

}