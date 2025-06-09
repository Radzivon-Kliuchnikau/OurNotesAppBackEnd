using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Interfaces;

public interface INoteRepository: IBaseRepository<Note, Guid>
{
    public Task<IEnumerable<Note>> GetNotesUserHaveAccessTo(string authorId);
}