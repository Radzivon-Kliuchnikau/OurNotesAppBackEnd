using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Note> Notes { get; init; }

    public DbSet<Product> Products { get; init; }

    public DbSet<Comment> Comments { get; init; }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);
    //
    //     modelBuilder.Entity<Note>();
    // }
}