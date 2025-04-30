using OurNotesAppBackEnd.Models;
using OurNotesAppBackEnd.Utils;

namespace OurNotesAppBackEnd.Interfaces;

public interface IProductRepository : IBaseRepository<Product, Guid>
{
    Task<IEnumerable<Product>> GetAllEntitiesAsync(ProductQueryObject? query);
}