using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Data;

public class NotesAppDbContext : DbContext
{
    public DbSet<Note> Notes { get; init; }
    
    public NotesAppDbContext(DbContextOptions<NotesAppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Note>();
    }
}