namespace OurNotesAppBackEnd.Data.Repository;

public interface IRepository<T, K>
{
    IEnumerable<T> GetAllEntities();

    T? GetEntityById(K id);

    void AddEntity(T entity);

    void EditEntity(T entity);

    void DeleteEntity(T entity);
}