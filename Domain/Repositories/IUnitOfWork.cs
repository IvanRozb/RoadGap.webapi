namespace Domain.Repositories;

public interface IUnitOfWork
{
    Task<int> SaveChanges(CancellationToken cancellationToken = default);
}