using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Interfaces;

public interface INoteRepository: IBaseRepository<Note, string>
{
}