namespace OurNotesAppBackEnd.Data.Repository;

public interface IBaseRepository<T, K>
{
    Task<IEnumerable<T>> GetAllEntitiesAsync();

    Task<T?> GetEntityByIdAsync(K id);

    Task AddEntityAsync(T entity);

    Task EditEntity(T entity);

    Task DeleteEntity(T entity);
}