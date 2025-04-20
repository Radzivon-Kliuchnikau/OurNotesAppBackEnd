using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Interfaces;

public interface IProductRepository : IBaseRepository<Product, Guid>
{
}