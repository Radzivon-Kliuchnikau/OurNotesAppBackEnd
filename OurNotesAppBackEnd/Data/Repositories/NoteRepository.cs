using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Data.Repositories;

public class NoteRepository(ApplicationDbContext context) : BaseRepository<Note, Guid>(context), INoteRepository;