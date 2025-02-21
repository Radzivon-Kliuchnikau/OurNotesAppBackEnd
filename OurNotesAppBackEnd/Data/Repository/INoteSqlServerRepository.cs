using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Data.Repository;

public interface INoteSqlServerRepository : IRepository<Note, string>
{
}