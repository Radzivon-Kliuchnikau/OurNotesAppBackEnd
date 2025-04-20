using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Data.Repositories;

public class CommentRepository(ApplicationDbContext context) : BaseRepository<Comment, Guid>(context), ICommentRepository;