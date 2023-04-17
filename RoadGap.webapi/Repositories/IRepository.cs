namespace RoadGap.webapi.Repositories;

public interface IRepository
{
    public void SaveChanges();
    public void AddEntity<T>(T entity);
    public void RemoveEntity<T>(T entity);
}