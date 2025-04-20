using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Interfaces;

public interface ICommentRepository : IBaseRepository<Comment, Guid>
{
}