using OurNotesAppBackEnd.Models;
using OurNotesAppBackEnd.Utils;

namespace OurNotesAppBackEnd.Interfaces;

public interface IBaseRepository<T, K> where T : BaseModel
{
    Task<IEnumerable<T>> GetAllEntitiesAsync();

    Task<T?> GetEntityByIdAsync(K id);

    Task AddEntityAsync(T entity);

    Task EditEntity(T entity);

    Task DeleteEntity(T entity);
}